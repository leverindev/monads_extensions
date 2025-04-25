using System;

namespace MonadsExtensions.Pipelines
{
    public class StringToNumberProcessorExample
    {
        public void Run()
        {
            var pipe = new Parser()
                .PipeTo(new Divider())
                .PipeTo(new Multiplier())
                .PipeTo(new Power());

            var producer = new CollectionProducer<string>();
            var consumer = new CollectionConsumer<int>();

            producer.Push("1");
            producer.Push("2");
            producer.Push("3");
            producer.Push("5");
            producer.Push("not a number");

            producer.Complete();

            PipeToExtensions.Create<string, int>()
                .AddProducer(producer)
                .AddConsumer(consumer)
                .AddPipe(pipe)
                .Run();

            foreach (var value in consumer.GetResult())
            {
                Console.WriteLine(value);
            }
        }
    }

    public class Parser : IPipeline<string, int>
    {
        public int Execute(string input)
        {
            return int.TryParse(input, out var value) ? value : 0;
        }
    }

    public class Divider : IPipeline<int, int>
    {
        public int Execute(int input)
        {
            return input / 2;
        }
    }

    public class Multiplier : IPipeline<int, int>
    {
        public int Execute(int input)
        {
            return input * 3;
        }
    }

    public class Power : IPipeline<int, int>
    {
        public int Execute(int input)
        {
            return input * input;
        }
    }
}
