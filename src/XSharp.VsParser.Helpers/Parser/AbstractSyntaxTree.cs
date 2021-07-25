﻿using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Parser
{
    public class AbstractSyntaxTree : IEnumerable<IParseTree>
    {
        readonly ParserHelper _ParserHelper;
        TokenStreamRewriter _TokenStreamRewriter = null;


        public TokenStreamRewriter Rewriter
        {
            get => _TokenStreamRewriter ??= new TokenStreamRewriter(_ParserHelper._Tokens);
        }

        public void Clear()
        {
            _TokenStreamRewriter = null;
        }

        internal AbstractSyntaxTree(ParserHelper parserHelper)
        {
            _ParserHelper = parserHelper;
        }

        IEnumerator<IParseTree> GetEnumerator(IParseTree start)
        {
            yield return start;
            for (int i = 0; i < start.ChildCount; i++)
            {
                var childEnumerator = GetEnumerator(start.GetChild(i));
                while (childEnumerator.MoveNext())
                    yield return childEnumerator.Current;
            }
        }

        public IEnumerator<IParseTree> GetEnumerator()
        {
            if (_ParserHelper._StartRule == null)
                throw new ArgumentException("Parsing was not successful");

            return GetEnumerator(_ParserHelper._StartRule);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }
}
