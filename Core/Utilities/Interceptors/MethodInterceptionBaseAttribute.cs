using Castle.DynamicProxy;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Core.Utilities.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        //Priority öncelik demek hangi önceliğin önce çalışacağını belirler(örenğin Önce validation mı loglama mı )
        public int Priority { get; set; }

        public virtual void Intercept(IInvocation invocation)
        {

        }
    }
}
