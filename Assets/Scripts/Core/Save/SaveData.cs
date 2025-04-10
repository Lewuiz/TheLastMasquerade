using System;
using System.Collections.Generic;

namespace Main
{
    public abstract class SaveData
    {
        public abstract Dictionary<string, object> GetDefaultValue();

        public abstract string GetSaveDataId();
    }
}
