//using System;
//using System.Collections.Generic;
//using TauCode.Parsing.Aide.Results;
//using TauCode.Parsing.Units;
//using TauCode.Parsing.Units.Impl;

//namespace TauCode.Parsing.Aide.Building
//{
//    public class BlockBuildingContext
//    {
//        private readonly IReadOnlyDictionary<string, BlockBuildingContext> _family;
//        private readonly List<UnitReference> _unitReferences;

//        public BlockBuildingContext(
//            BlockDefinitionResult mold,
//            IReadOnlyDictionary<string, BlockBuildingContext> family)
//        {
//            // todo: arg checks

//            this.Mold = mold;
//            _family = family;
//            _unitReferences = new List<UnitReference>();
//        }

//        public string BlockName => this.Mold.GetBlockDefinitionResultName();
//        public bool IsTop => throw new NotImplementedException();
//        public bool IsClone => throw new NotImplementedException();
//        public BlockDefinitionResult Mold { get; }
//        public IBlock Block => throw new NotImplementedException();
//        public IReadOnlyDictionary<string, BlockBuildingContext> Family => _family;
//        public IReadOnlyList<UnitReference> UnitReferences => _unitReferences;
//        public void AddUnitReference(Node referencingNode, string holdingBlockName, string referencedUnitName)
//        {
//            var unitReference = new UnitReference(referencingNode, holdingBlockName, referencedUnitName);
//            _unitReferences.Add(unitReference);
//        }
//    }
//}
