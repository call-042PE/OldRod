// Project OldRod - A KoiVM devirtualisation utility.
// Copyright (C) 2019 Washi
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using OldRod.Core.Architecture;

namespace OldRod.Core.Ast.IL
{
    public abstract class ILExpression : ILAstNode
    {
        private ILFlagsVariable _flagsVariable;

        protected ILExpression(VMType expressionType)
        {
            ExpressionType = expressionType;
        }
        
        public VMType ExpressionType
        {
            get;
            set;
        }

        public abstract bool HasPotentialSideEffects
        {
            get;
        }

        public ILFlagsVariable FlagsVariable
        {
            get => _flagsVariable;
            set
            {
                _flagsVariable?.ImplicitAssignments.Remove(this);
                _flagsVariable = value;
                _flagsVariable?.ImplicitAssignments.Add(this);
            }
        }

        public bool IsFlagDataSource => FlagsVariable != null;
    }
}