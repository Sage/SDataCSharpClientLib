using System;
using System.ComponentModel;

namespace Sage.SData.Client.Core
{
    public class ReadCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly object _result;

        public ReadCompletedEventArgs(object result, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            _result = result;
        }

        public object Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _result;
            }
        }
    }
}