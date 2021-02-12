using System;

namespace Tao.Project.Console
{
    /// <summary>
    /// 配置选项
    /// </summary>
    public class MetricsCollectionOptions
    {
        public TimeSpan CaptureInterval { get; set; }
        public TransportType Transport { get; set; }
        public Endpoint DeliverTo { get; set; }
    }

    public enum TransportType
    {
        Tcp,
        Http,
        Udp
    }

    public class Endpoint
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}