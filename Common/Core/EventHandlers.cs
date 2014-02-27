using System;

namespace IntegrationService
{
    public class JobEventArgs<TQueueObj> : EventArgs
        //where TQueueObj : class
    {
        public JobEventArgs(JobBase<TQueueObj> j)
        {
            job = j;
        }
        public JobBase<TQueueObj> job;
    }

    public class ObjectsPickedEventArgs : EventArgs
    {
        public ObjectsPickedEventArgs(int count)
        {
            itemsCount = count;
        }
        public int itemsCount;
    }

    public class ObjectEnquedEventArgs : EventArgs
    {
        public ObjectEnquedEventArgs(object obj)
        {
            Obj = obj;
        }
        public object Obj;
    }

    public class ObjectProcessErrorEventArgs : EventArgs
    {
        public ObjectProcessErrorEventArgs(object obj, Exception ex)
        {
            Obj = obj;
            Ex = ex;
        }
        public object Obj;
        public Exception Ex;
    }

    public class ObjectsPickErrorEventArgs : EventArgs
    {
        public ObjectsPickErrorEventArgs(Exception ex)
        {
            Ex = ex;
        }
        public Exception Ex;
    }

    public class ObjectProcessedEventArgs : EventArgs
    {
        public ObjectProcessedEventArgs(object obj, TimeSpan time)
        {
            Obj = obj;
            Time = time;
        }
        public object Obj;
        public TimeSpan Time;
    }
}