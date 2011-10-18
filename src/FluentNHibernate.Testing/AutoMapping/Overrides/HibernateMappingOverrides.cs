﻿using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Overrides
{
    [TestFixture]
    public class HibernateMappingOverrides
    {
        [Test, Ignore]
        public void CanOverrideDefaultLazy()
        {
            AutoPersistenceModel model = AutoMap.Source(new StubTypeSource(new[] {typeof(Parent)}))
                .Override<Parent>(o => o.HibernateMapping.Not.DefaultLazy());

            HibernateMapping hibernateMapping = model.BuildMappings().First();

            hibernateMapping.DefaultLazy.ShouldBeFalse();
        }
    }
}