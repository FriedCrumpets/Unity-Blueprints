namespace Blueprints.BehaviourTree
{
    //todo: this class has a lot of protected bits... I don't like that... fix this to be more compositionally organised
    //todo: ^^ would this overwork garbage collection if I was to do this?? I've done it... woops test both
    public class Branch : INode
    {
        private readonly ILeaf leaf;

        public Branch(ILeaf leaf, Tree root = null)
        {
            this.leaf = leaf;
            
            if(root != null)
                this.leaf.Root = root;
        }
        
        public Result Execute()
        {
            if (leaf.RequireInitialisation)
                if (leaf.Init() == Result.Failure)
                    return Result.Failure;

            return leaf.Process();
        }
    }
}