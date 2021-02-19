namespace Tao.Project.IOC
{
    /// <summary>
    /// 服务的生命周期
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// 多个同根实例范围内确保提供的服务是单例的
        /// </summary>
        Root,

        /// <summary>
        /// 某个容器范围内提供的服务是单例的
        /// </summary>
        Self,

        /// <summary>
        /// 每次都会创建一个新的实例
        /// </summary>
        Transient
    }
}