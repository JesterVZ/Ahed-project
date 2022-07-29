using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class CancellationTokenService
    {
        private CancellationToken _token;
        private CancellationTokenSource _source;
        public CancellationTokenService()
        {
            _source = new CancellationTokenSource();
            _token = _source.Token;
        }
        public CancellationToken GetToken()
        {
            return _token;
        }

        public void ReCreateSource()
        {
            if (_source.IsCancellationRequested)
            {
                _source.Dispose();
                _source = new CancellationTokenSource();
                _token = _source.Token;
            }
        }

        public void Stop()
        {
            _source.Cancel();
        }
    }
}
