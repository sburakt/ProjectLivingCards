using System.Collections.Generic;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Presenter
{
    public struct EventSnapshot
    {
        public MatchEvent MatchEvent;
        public CardSnapshot CardSnapshot;
    }
}