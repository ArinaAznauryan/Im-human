using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdditionalEventHandler {
    public abstract class AdditionalEventHandler: MonoBehaviour
    {
        public bool done = false;

        public abstract void additionalEventTrigger();
        public abstract void annulParameters();
    }
}
