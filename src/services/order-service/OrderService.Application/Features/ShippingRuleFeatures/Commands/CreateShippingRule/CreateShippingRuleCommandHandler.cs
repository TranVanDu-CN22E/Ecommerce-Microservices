using ExcelDataReader;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Aggregates.ShippingRuleAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.ShippingRuleFeatures.Commands.CreateShippingRule
{
    public class CreateShippingRuleCommandHandler : ICommandHandler<CreateShippingRuleCommand, Result<bool>>
    {
        private IShippingRuleRepository _shippingRuleRepository;
        public CreateShippingRuleCommandHandler(IShippingRuleRepository shippingRuleRepository)
        {
            _shippingRuleRepository = shippingRuleRepository;
        }
        public async Task<Result<bool>> Handle(CreateShippingRuleCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return Result<bool>.Failure(new[] { ShippingRuleErrors.FileIsNotNull });
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var shippingRulesToCreate = new List<ShippingRule>();

            try
            {
                using (var stream = request.File.OpenReadStream())
                {
                    // Tự động nhận diện file .xls hoặc .xlsx
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var isHeaderRow = true;

                        // Đọc từng dòng của file Excel
                        while (reader.Read())
                        {
                            // Bỏ qua dòng tiêu đề đầu tiên (Province | Fee)
                            if (isHeaderRow)
                            {
                                isHeaderRow = false;
                                continue;
                            }

                            // Đọc dữ liệu từ cột số 0 (Province) và cột số 1 (Fee)
                            string province = reader.GetValue(0)?.ToString()?.Trim();
                            string feeRaw = reader.GetValue(1)?.ToString()?.Trim();
                            string freeThresholdRaw = reader.GetValue(2)?.ToString()?.Trim();
                            string currency = reader.GetValue(3)?.ToString()?.Trim();

                            // Kiểm tra dữ liệu dòng trống
                            if (string.IsNullOrEmpty(province) && string.IsNullOrEmpty(feeRaw))
                                continue;

                            if (!decimal.TryParse(feeRaw, out decimal fee))
                            {
                                return Result<bool>.Failure(new[] { new Error("InvalidFee", $"Dữ liệu chi phí '{feeRaw}' tại tỉnh {province} không hợp lệ.") });
                            }
                            if (!decimal.TryParse(freeThresholdRaw, out decimal freeThreshold))
                            {
                                return Result<bool>.Failure(new[] { new Error("InvalidFreeThreshold", $"Dữ liệu ngưỡng miễn phí '{freeThresholdRaw}' tại tỉnh {province} không hợp lệ.") });
                            }

                            var shippingRule = ShippingRule.Create(Province.Create(province), Money.Create(fee, currency), Money.Create(freeThreshold, currency));
                            shippingRulesToCreate.Add(shippingRule);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi tại đây nếu cần
                return Result<bool>.Failure(new[] { new Error("ExcelReadError", $"Đã xảy ra lỗi khi xử lý file Excel: {ex.Message}") });
            }
            // BƯỚC 2: Lưu vào DB (Nằm ngoài try-catch để TransactionBehavior hoạt động đúng)
            foreach (var rule in shippingRulesToCreate)
            {
                await _shippingRuleRepository.AddShippingRuleAsync(rule, cancellationToken);
            }

            return Result<bool>.Success(true);
        }
    }
}
