using System;

namespace Tao.Project.Console
{
    /// <summary>
    /// 性能指标
    /// </summary>
    public class PerformanceMetrics
    {
        private static readonly Random _random = new Random();

        public int Processor { get; set; }
        public long Memory { get; set; }
        public long Network { get; set; }

        public override string ToString()
        {
            return $"Cpu:{Processor}%,Memory:{Memory / (1024 * 1024)}/M,Network:{Network / (1024 * 1024)}/M/s";
        }

        public static PerformanceMetrics Create() =>
            new PerformanceMetrics()
            {
                Processor = _random.Next(1, 100),
                Memory = _random.Next(10, 1000) * 1024 * 1024,
                Network = _random.Next(10, 1000) * 2024 * 1024
            };
    }
}