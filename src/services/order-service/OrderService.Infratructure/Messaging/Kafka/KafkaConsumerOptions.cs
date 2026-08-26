namespace OrderService.Infratructure.Messaging.Kafka
{
    public class KafkaConsumerOptions
    {
        public string BootstrapServers { get; set; } = string.Empty;
        /// <summary>
        /// BẮT BUỘC: Mỗi service phải có GroupId riêng.
        /// Nhiều instance cùng GroupId → Kafka auto balance partitions.
        /// </summary>
        public string GroupId { get; set; } = string.Empty;
        /// <summary>
        /// Earliest: đọc từ đầu topic nếu chưa có offset
        /// Latest: chỉ đọc message mới sau khi subscribe
        /// </summary>
        public string AutoOffsetReset { get; set; } = "Earliest";
        /// <summary>
        /// Tắt auto commit → manual commit sau khi xử lý thành công.
        /// Tránh mất message khi crash giữa chừng.
        /// </summary>
        public bool EnableAutoCommit { get; set; } = false;
        /// <summary>
        /// Max thời gian (ms) giữa 2 lần Consume(). 
        /// Nếu handler xử lý lâu hơn giá trị này → broker coi consumer chết → rebalance.
        /// Set >= max thời gian xử lý 1 batch.
        /// </summary>
        public int MaxPollIntervalMs { get; set; } = 300000; // 5 phút
        /// <summary>
        /// Broker coi consumer chết nếu không heartbeat trong khoảng này.
        /// </summary>
        public int SessionTimeoutMs { get; set; } = 30000;
        /// <summary>
        /// Thời gian chờ trước khi reconnect khi mất kết nối.
        /// </summary>
        public int ReconnectBackoffMs { get; set; } = 1000;
    }
}
