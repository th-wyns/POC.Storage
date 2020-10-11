using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Storage
{
    enum State { InProgress, Failed, Succeeded }

    /// <summary>
    /// Transaction Orchestrator Implementation
    /// </summary>
    public class TransactionOrchestrator
    {
        IList<ITransaction> Transactions { get; } = new List<ITransaction>();
        IList<Action<State>> TransactionCallbacks { get; set; } = new List<Action<State>>();
        State State { get; } = State.InProgress;

    }
}
