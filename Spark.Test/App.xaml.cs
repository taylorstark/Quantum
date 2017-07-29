using System.Reflection;

namespace Spark.Test
{
    sealed partial class App
    {
        protected override void OnInitializeRunner()
        {
            AddTestAssembly(GetType().GetTypeInfo().Assembly);
        }
    }
}
