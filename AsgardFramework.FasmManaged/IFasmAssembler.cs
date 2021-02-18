using System.Collections.Generic;
using System.IO;

namespace AsgardFramework.FasmManaged
{
    public interface IFasmAssembler
    {
        /// <summary>
        /// Assembles the given source
        /// </summary>
        /// <param name="instructions">Source</param>
        /// <param name="architecture">Specifies target architecture of generated code</param>
        /// <param name="output">The pipe, to which the output of DISPLAY directives will be written. If this parameter is <see langword="null"/>, all the display will get discarded.</param>
        /// <returns>The generated output</returns>
        /// <exception cref="FasmException"/>
        public IEnumerable<byte> Assemble(IEnumerable<string> instructions, TargetArchitecture architecture = TargetArchitecture.x86, TextWriter output = null);

        /// <summary>
        /// Assembles the given source
        /// </summary>
        /// <param name="instructions">Source</param>
        /// <param name="architecture">Specifies target architecture of generated code</param>
        /// <param name="output">The pipe, to which the output of DISPLAY directives will be written. If this parameter is <see langword="null"/>, all the display will get discarded.</param>
        /// <returns>The generated output</returns>
        /// <exception cref="FasmException"/>
        public IEnumerable<byte> Assemble(string instructions, TargetArchitecture architecture = TargetArchitecture.x86, TextWriter output = null);

        /// <summary>
        /// Assembles the given source
        /// </summary>
        /// <param name="filePath">Source file path</param>
        /// <param name="output">The pipe, to which the output of DISPLAY directives will be written. If this parameter is <see langword="null"/>, all the display will get discarded.</param>
        /// <returns>The generated output</returns>
        /// <exception cref="FasmException"/>
        public IEnumerable<byte> AssembleFile(string filePath, TextWriter output = null);

        /// <summary>
        /// Major and minor version
        /// </summary>
        public string FasmDLLVersion { get; }
    }
}
