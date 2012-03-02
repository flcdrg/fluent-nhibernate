using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
#pragma warning disable 612,618,672
    public class AutoJoinedSubClassPart<T> : JoinedSubClassPart<T>, IAutoClasslike
    {
        readonly IList<Member> mappedMembers = new List<Member>();
        readonly MappingProviderStore providers;

        public AutoJoinedSubClassPart(string keyColumn)
            : this(keyColumn, new MappingProviderStore())
        {}

        AutoJoinedSubClassPart(string keyColumn, MappingProviderStore providers)
            : base(keyColumn, new AttributeStore(), providers)
        {
            this.providers = providers;
        }

        void IAutoClasslike.DiscriminateSubClassesOnColumn(string column)
        {}

        void IAutoClasslike.AlterModel(ClassMappingBase mapping)
        {}

        public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
        {
            Type genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(type);
            var joinedclass = (ISubclassMappingProvider)Activator.CreateInstance(genericType, keyColumn);

            providers.Subclasses[type] = joinedclass;

            return (IAutoClasslike)joinedclass;
        }

        public IAutoClasslike SubClass(Type type, string discriminatorValue)
        {
            Type genericType = typeof(AutoSubClassPart<>).MakeGenericType(type);
            var subclass = (ISubclassMappingProvider)Activator.CreateInstance(genericType, discriminatorValue);

            providers.Subclasses[type] = subclass;

            return (IAutoClasslike)subclass;
        }

        public ClassMapping GetClassMapping()
        {
            return null;
        }

        public HibernateMapping GetHibernateMapping()
        {
            return null;
        }

        public IEnumerable<Member> GetIgnoredProperties()
        {
            return mappedMembers;
        }

        public object GetMapping()
        {
            return ((ISubclassMappingProvider)this).GetSubclassMapping();
        }

        protected override OneToManyPart<TChild> HasMany<TChild>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasMany<TChild>(property);
        }

        protected override PropertyPart Map(Member property, string columnName)
        {
            mappedMembers.Add(property);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<TOther> References<TOther>(Member property, string columnName)
        {
            mappedMembers.Add(property);
            return base.References<TOther>(property, columnName);
        }

        protected override ManyToManyPart<TChild> HasManyToMany<TChild>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasManyToMany<TChild>(property);
        }

        protected override ComponentPart<TComponent> Component<TComponent>(Member property, Action<ComponentPart<TComponent>> action)
        {
            mappedMembers.Add(property);
            return base.Component(property, action);
        }

        protected override ReferenceComponentPart<TComponent> Component<TComponent>(Member property)
        {
            mappedMembers.Add(property);
            return base.Component<TComponent>(property);
        }

        protected override OneToOnePart<TOther> HasOne<TOther>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasOne<TOther>(property);
        }

        public void JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
        {
            Type genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var joinedclass = (AutoJoinedSubClassPart<TSubclass>)Activator.CreateInstance(genericType, keyColumn);

            action(joinedclass);

            providers.Subclasses[typeof(TSubclass)] = joinedclass;
        }

        public void SubClass<TSubclass>(string discriminatorValue, Action<AutoSubClassPart<TSubclass>> action)
        {
            Type genericType = typeof(AutoSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var subclass = (AutoSubClassPart<TSubclass>)Activator.CreateInstance(genericType, discriminatorValue);

            action(subclass);

            providers.Subclasses[typeof(TSubclass)] = subclass;
        }
    }
#pragma warning restore 612,618,672
}