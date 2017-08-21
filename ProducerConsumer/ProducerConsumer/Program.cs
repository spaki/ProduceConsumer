using System;
using System.Threading.Tasks;

namespace Poc
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new ItemHandler<Item>(DoThings);

            Parallel.For(0, 1000, i => {
                handler.AddItem(new Item { Id = Guid.NewGuid(), Number = i });
            });

            Console.WriteLine();
            Console.WriteLine("end...");
            Console.ReadLine();
        }

        static void DoThings(Item item)
        {
            Console.WriteLine($"item number {item.Number}, id {item.Id}");
        }
    }

    class Item
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
    }
}