#if !(UNITY_WSA && ENABLE_DOTNET)

using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using System.Linq;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Convention
{
    [TestFixture]
    public class TestConvention : ZenjectUnitTestFixture
    {
        [Test]
        public void TestDerivingFrom()
        {
            Container.Bind<IFoo>()
                .To(x => x.AllTypes().DerivingFrom<IFoo>().FromThisAssembly());

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count(), 4);
        }

        [Test]
        public void TestDerivingFrom2()
        {
            Container.Bind<IFoo>()
                .To(x => x.AllTypes().DerivingFrom<IFoo>());

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count(), 4);
        }

        [Test]
        public void TestMatchAll()
        {
            // Should automatically filter by contract types
            Container.Bind<IFoo>().To(x => x.AllNonAbstractClasses());

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count(), 4);
        }

#if !NOT_UNITY3D
        [Test]
        public void TestDerivingFromFail()
        {
            Container.Bind<IFoo>()
                .To(x => x.AllTypes().DerivingFrom<IFoo>().FromAssemblyContaining<UnityEngine.Vector3>());

            Assert.That(Container.ResolveAll<IFoo>().IsEmpty());
        }
#endif

        [Test]
        public void TestAttributeFilter()
        {
            Container.Bind<IFoo>()
                .To(x => x.AllTypes().WithAttribute<ConventionTestAttribute>());

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count(), 2);
        }

        [Test]
        public void TestAttributeWhereFilter()
        {
            Container.Bind<IFoo>()
                .To(x => x.AllTypes().WithAttributeWhere<ConventionTestAttribute>(a => a.Num == 1));

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count(), 1);
        }

        [Test]
        public void TestInNamespace()
        {
            Container.Bind<IFoo>()
                .To(x => x.AllTypes().DerivingFrom<IFoo>().InNamespace("Zenject.Tests.Convention.NamespaceTest"));

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count(), 1);
        }
    }
}

#endif
