using System;

namespace Common.ServiceModel.Tasks
{
    public class ProcessEventArgs : EventArgs
    {
        public ProcessEventArgs(float percent)
        {
            Percent = percent;
        }

        /// <summary>
        /// Процент выполнения. Может иметь значение от 0 до 1.
        /// </summary>
        public float Percent { get; private set; }
    }
}