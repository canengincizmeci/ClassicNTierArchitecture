using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }
        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            //Metot Çalışmadan önce
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                isSuccess = false;
                //Hata aldığında
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    //İşlem başarıyla
                    OnSuccess(invocation);
                }
            }
            //En son
            OnAfter(invocation);
        }
    }
}
