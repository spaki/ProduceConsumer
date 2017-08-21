using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Poc
{
    public class ItemHandler<T>
    {
        private List<Thread> consumerThreads = new List<Thread>();
        private BlockingCollection<T> queue = new BlockingCollection<T>();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Action<T> processReceivingItem;

        public ItemHandler(Action<T> processReceivingItem, int maxConsumerThreads = 2)
        {
            this.processReceivingItem = processReceivingItem;
            this.StartConsumers(maxConsumerThreads);
        }

        public void AddItem(T item)
        {
            queue.Add(item);
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }

        private void StartConsumers(int maxConsumerThreads)
        {
            for (int i = 0; i < maxConsumerThreads; i++)
            {
                var consumerThread = new Thread(Consume);

                consumerThreads.Add(consumerThread);

                consumerThread.IsBackground = true;
                consumerThread.Start();
            }
        }

        private void Consume()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                T item = queue.Take(cancellationTokenSource.Token);
                processReceivingItem(item);
            }
        }
    }
}