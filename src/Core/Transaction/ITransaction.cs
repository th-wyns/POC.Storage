using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Storage
{
    interface ITransaction
    {
        void Start();
        void Commit();
        void Rollback();
    }
}
