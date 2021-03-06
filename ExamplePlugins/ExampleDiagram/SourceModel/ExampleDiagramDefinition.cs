﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NationalInstruments.SourceModel;
using NationalInstruments.SourceModel.Envoys;
using NationalInstruments.SourceModel.Persistence;

namespace ExamplePlugins.ExampleDiagram.SourceModel
{
    public class ExampleDiagramDefinition : DiagramDefinition
    {
        /// <summary>
        /// This is the identifier used to tie this definition to the Document type which enables the
        /// editing of the definition.
        /// </summary>
        public static readonly EnvoyKeyword DefinitionType = new EnvoyKeyword(ElementName, ExamplePluginsNamespaceSchema.ParsableNamespaceName);

        /// <summary>
        /// This is the specific type identifier for the definition
        /// </summary>
        public const string ElementName = "ExampleDiagramDefinition";

        /// <summary>
        /// The constructor for the definition.  It is protected to avoid usage.
        /// New definitions should be created by calling the static Create method.
        /// </summary>
        protected ExampleDiagramDefinition()
        {
        }

        protected override void CreateBatchRules(ICollection<ModelBatchRule> rules)
        {
            base.CreateBatchRules(rules);

            rules.Add(new CoreBatchRule());
            rules.Add(new VerticalGrowNodeBoundsRule());
            rules.Add(new WiringBatchRule());
            rules.Add(new WireCommentBatchRule());
        }

        protected override void Init(IElementCreateInfo info)
        {
            if (!info.ForParse)
            {
                RootDiagram = new ExampleRootDiagram();
            }
            base.Init(info);
        }

        /// <summary>
        /// Gets or sets the root diagram of the function. Child classes will likely want to expose a public property
        /// that wraps this property but is of a more specific class as needed for that model of computation.
        /// </summary>
        public ExampleRootDiagram RootDiagram
        {
            get
            {
                return Components.OfType<ExampleRootDiagram>().FirstOrDefault();
            }
            protected set
            {
                var components = ComponentsForModify;
                ExampleRootDiagram current = components.OfType<ExampleRootDiagram>().FirstOrDefault();
                if (current != null)
                {
                    components.Remove(current);
                }
                components.Add(value);
            }
        }
        
        /// <summary>
        /// Returns the persistence name of this node
        /// </summary>
        public override XName XmlElementName
        {
            get
            {
                return XName.Get(ElementName, ExamplePluginsNamespaceSchema.ParsableNamespaceName);
            }
        }

        /// <summary>
        ///  String used to identify the document type in things such as SourceFileReference.
        /// </summary>
        /// <remarks>Overriding this is a good idea for child classes. By overriding and returning a constant,
        /// you will improve performance by avoiding the expensive reflection.</remarks>
        public override EnvoyKeyword ModelDefinitionType
        {
            get
            {
                return DefinitionType;
            }
        }

        private IWiringBehavior _wiringBehavior = new ManhattanWiringBehavior();

        public override IWiringBehavior DefaultWiringBehavior
        {
            get
            {
                return _wiringBehavior;
            }
        }

        /// <summary>
        /// Our create this.  This is used to create a new instance either programmatically, from load, and from the palette
        /// </summary>
        /// <param name="elementCreateInfo">creation information.  This tells us why we are being created (new, load, ...)</param>
        /// <returns>The newly created definition</returns>
        [XmlParserFactoryMethod(ElementName, ExamplePluginsNamespaceSchema.ParsableNamespaceName)]
        [ExportDefinitionFactory(ElementName, ExamplePluginsNamespaceSchema.ParsableNamespaceName)]
        public static ExampleDiagramDefinition Create(IElementCreateInfo elementCreateInfo)
        {
            var definition = new ExampleDiagramDefinition();
            definition.Init(elementCreateInfo);
            return definition;
        }
    }
}
