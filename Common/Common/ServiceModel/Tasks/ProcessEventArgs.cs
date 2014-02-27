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
        /// ������� ����������. ����� ����� �������� �� 0 �� 1.
        /// </summary>
        public float Percent { get; private set; }
    }
}