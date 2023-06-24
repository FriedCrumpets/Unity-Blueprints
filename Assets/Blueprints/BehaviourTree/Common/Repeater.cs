namespace Blueprints.BehaviourTree
{
    /// <summary>
    /// Repeats child node; will repeat infinitely if <see cref="_repetitions"/> is set to 0. Can be killed early with
    /// <see cref="Kill"/>
    /// </summary>
    public class Repeater : Decorator
    {
        private readonly int _repetitions;
        private int _repeated;
        private bool kill;

        public Repeater(INode node, int repetitions = 0) : base(node)
        {
            _repetitions = repetitions;
            _repeated = 0;
        }
        
        public override Result Execute()
        {
            if (Repeat() && !kill)
            {
                Child.Execute();
                return Result.Running;
            }

            return Result.Success;
        }

        public void Kill()
            => kill = true;

        private bool Repeat()
            => _repetitions == 0 || _repetitions > _repeated++;
    }
}