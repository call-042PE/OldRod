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

using System.Collections.Generic;
using AsmResolver.Net.Cil;

namespace OldRod.Core.CodeGen.Blocks
{
    public class BasicBlock : Block
    {
        public BasicBlock(IEnumerable<CilInstruction> instructions)
        {
            Instructions = new List<CilInstruction>(instructions);
        }

        public IList<CilInstruction> Instructions
        {
            get;
        }

        public override IList<CilInstruction> GenerateInstructions()
        {
            return Instructions;
        }

        public override string ToString()
        {
            return $"{{\n{string.Join("\n", Instructions)}\n}}";
        }
    }
}