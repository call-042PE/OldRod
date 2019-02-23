using System;
using System.IO;
using AsmResolver.Net.Cts;
using AsmResolver.Net.Metadata;
using OldRod.Core.CodeGen;
using OldRod.Core.Ast.Cil;
using OldRod.Core.Recompiler;
using Rivers.Serialization.Dot;

namespace OldRod.Pipeline.Stages.Recompiling
{
    public class RecompilerStage : IStage
    {
        public const string Tag = "Recompiler";
        
        public string Name => "CIL Recompiler stage";

        public void Run(DevirtualisationContext context)
        {
            foreach (var method in context.VirtualisedMethods)
            {
                var recompiler = new ILToCilRecompiler(method.CallerMethod.CilMethodBody, context.TargetImage);
                
                context.Logger.Debug(Tag, $"Recompiling export {method.ExportId}...");
                method.CilCompilationUnit = (CilCompilationUnit) method.ILCompilationUnit.AcceptVisitor(recompiler);
                
                var generator = new CilMethodBodyGenerator(context.TargetImage, context.Constants);
                method.CallerMethod.CilMethodBody = generator.Compile(method.CallerMethod, method.CilCompilationUnit);

                if (context.Options.DumpControlFlowGraphs)
                {
                    context.Logger.Log(Tag, $"Dumping CIL Ast of export {method.ExportId}...");
                    DumpCilAst(context, method);
                }
            }
        }

        private static void DumpCilAst(DevirtualisationContext context, VirtualisedMethod method)
        {
            method.CallerMethod.CilMethodBody.Instructions.CalculateOffsets();

            using (var fs = File.CreateText(Path.Combine(context.Options.OutputDirectory, $"export{method.ExportId}_cilast.dot")))
            {
                var writer = new DotWriter(fs, new BasicBlockSerializer(method.CallerMethod.CilMethodBody));
                writer.Write(Utilities.ConvertToGraphViz(method.ControlFlowGraph, CilAstBlock.AstBlockProperty));
            }
        }
    }
}