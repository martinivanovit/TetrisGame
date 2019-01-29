using System;

namespace TetrisGame
{
    public class EnginePropertyChangedArgs : EventArgs
    {
        public string PropertyName { get; set; }
        public object NewValue { get; set; }

        public EnginePropertyChangedArgs(string propertyName, object newValue)
        {
            this.PropertyName = propertyName;
            this.NewValue = newValue;
        }
    }
}
