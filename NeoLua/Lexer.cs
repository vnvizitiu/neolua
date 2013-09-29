﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TecWare.Core.Compile;

namespace Neo.IronLua
{
  #region -- enum LuaToken ------------------------------------------------------------

  ///////////////////////////////////////////////////////////////////////////////
  /// <summary>Tokens</summary>
  internal enum LuaToken
  {
    /// <summary>Nicht definierter Token</summary>
    None,
    /// <summary>Ende der Datei.</summary>
    Eof,

    /// <summary>Ungültiges Zeichen</summary>
    InvalidChar,
    /// <summary>Ungültige Zeichenkette</summary>
    InvalidString,
    /// <summary>Ungültiger Kommentar</summary>
    InvalidComment,

    /// <summary>Zeilenumbruch</summary>
    NewLine,
    /// <summary>Leerzeichen</summary>
    Whitespace,
    /// <summary>Kommentar</summary>
    Comment,
    /// <summary>Zeichenkette</summary>
    String,
    /// <summary>Zahl</summary>
    Number,
    /// <summary>Zahl</summary>
    HexNumber,
    /// <summary>Bezeichner.</summary>
    Identifier,

    /// <summary>Schlüsselwort and</summary>
    KwAnd,
    /// <summary>Schlüsselwort break</summary>
    KwBreak,
    /// <summary>Schlüsselwort cast</summary>
    KwCast,
    /// <summary>Schlüsselwort do</summary>
    KwDo,
    /// <summary>Schlüsselwort else</summary>
    KwElse,
    /// <summary>Schlüsselwort elseif</summary>
    KwElseif,
    /// <summary>Schlüsselwort end</summary>
    KwEnd,
    /// <summary>Schlüsselwort false</summary>
    KwFalse,
    /// <summary>Schlüsselwort for</summary>
    KwFor,
    /// <summary>Schlüsselwort function</summary>
    KwFunction,
    /// <summary>Schlüsselwort goto</summary>
    KwGoto,
    /// <summary>Schlüsselwort if</summary>
    KwIf,
    /// <summary>Schlüsselwort in</summary>
    KwIn,
    /// <summary>Schlüsselwort local</summary>
    KwLocal,
    /// <summary>Schlüsselwort nil</summary>
    KwNil,
    /// <summary>Schlüsselwort not</summary>
    KwNot,
    /// <summary>Schlüsselwort or</summary>
    KwOr,
    /// <summary>Schlüsselwort repeat</summary>
    KwRepeat,
    /// <summary>Schlüsselwort return</summary>
    KwReturn,
    /// <summary>Schlüsselwort then</summary>
    KwThen,
    /// <summary>Schlüsselwort true</summary>
    KwTrue,
    /// <summary>Schlüsselwort until</summary>
    KwUntil,
    /// <summary>Schlüsselwort while</summary>
    KwWhile,

    /// <summary>+</summary>
    Plus,
    /// <summary>-</summary>
    Minus,
    /// <summary>*</summary>
    Star,
    /// <summary>/</summary>
    Slash,
    /// <summary>%</summary>
    Percent,
    /// <summary>^</summary>
    Caret,
    /// <summary>#</summary>
    Cross,
    /// <summary>=</summary>
    Equal,
    /// <summary>~=</summary>
    NotEqual,
    /// <summary>&lt;=</summary>
    LowerEqual,
    /// <summary>&gt;=</summary>
    GreaterEqual,
    /// <summary>&lt;</summary>
    Lower,
    /// <summary>&gt;</summary>
    Greater,
    /// <summary>=</summary>
    Assign,
    /// <summary>(</summary>
    BracketOpen,
    /// <summary>)</summary>
    BracketClose,
    /// <summary>{</summary>
    BracketCurlyOpen,
    /// <summary>}</summary>
    BracketCurlyClose,
    /// <summary>[</summary>
    BracketSquareOpen,
    /// <summary>]</summary>
    BracketSquareClose,
    /// <summary>;</summary>
    Semicolon,
    /// <summary>:</summary>
    Colon,
    /// <summary>::</summary>
    ColonColon,
    /// <summary>,</summary>
    Comma,
    /// <summary>.</summary>
    Dot,
    /// <summary>..</summary>
    DotDot,
    /// <summary>...</summary>
    DotDotDot
  } // enum LuaToken

  #endregion

  #region -- class LuaLexer -----------------------------------------------------------

  ///////////////////////////////////////////////////////////////////////////////
  /// <summary></summary>
  internal class LuaLexer : ScannerBase<LuaToken>
  {
    protected override bool IsComment(LuaToken iKind)
    {
      return iKind == LuaToken.Comment;
    } // func IsComment

    protected override bool IsNewLine(LuaToken iKind)
    {
      return iKind == LuaToken.NewLine;
    } // func IsNewLine

    protected override bool IsWhitespace(LuaToken iKind)
    {
      return iKind == LuaToken.Whitespace || iKind == LuaToken.NewLine;
    } // func IsWhitespace

    #region -- NextToken --------------------------------------------------------------

    protected override Token<LuaToken> NextToken()
    {
      char cStringMode = '\0';
      byte bChar = 0;
      while (true)
      {
        char c = Cur;

        switch (CurState)
        {
          #region -- 0 ----------------------------------------------------------------
          case 0:
            if (c == '\0')
              return CreateToken(0, LuaToken.Eof);
            else if (c == '\n')
              return NextCharAndCreateToken(0, LuaToken.NewLine);
            else if (Char.IsWhiteSpace(c))
              NextChar(10);

            else if (c == '+')
              return NextCharAndCreateToken(0, LuaToken.Plus);
            else if (c == '-')
              NextChar(50);
            else if (c == '*')
              return NextCharAndCreateToken(0, LuaToken.Star);
            else if (c == '/')
              return NextCharAndCreateToken(0, LuaToken.Slash);
            else if (c == '%')
              return NextCharAndCreateToken(0, LuaToken.Percent);
            else if (c == '^')
              return NextCharAndCreateToken(0, LuaToken.Caret);
            else if (c == '#')
              return NextCharAndCreateToken(0, LuaToken.Cross);
            else if (c == '=')
              NextChar(20);
            else if (c == '~')
              NextChar(21);
            else if (c == '<')
              NextChar(22);
            else if (c == '>')
              NextChar(23);
            else if (c == '(')
              return NextCharAndCreateToken(0, LuaToken.BracketOpen);
            else if (c == ')')
              return NextCharAndCreateToken(0, LuaToken.BracketClose);
            else if (c == '{')
              return NextCharAndCreateToken(0, LuaToken.BracketCurlyOpen);
            else if (c == '}')
              return NextCharAndCreateToken(0, LuaToken.BracketCurlyClose);
            else if (c == '[')
              NextChar(27);
            else if (c == ']')
              return NextCharAndCreateToken(0, LuaToken.BracketSquareClose);
            else if (c == ';')
              return NextCharAndCreateToken(0, LuaToken.Semicolon);
            else if (c == ':')
              NextChar(30);
            else if (c == ',')
              return NextCharAndCreateToken(0, LuaToken.Comma);
            else if (c == '.')
              NextChar(24);

            else if (c == '"')
            {
              cStringMode = c;
              NextChar(40);
            }
            else if (c == '\'')
            {
              cStringMode = c;
              NextChar(40);
            }

            else if (c == '0')
              EatChar(60);
            else if (c >= '1' && c <= '9')
              EatChar(61);

            else if (c == 'a')
              EatChar(1010);
            else if (c == 'b')
              EatChar(1020);
            else if (c == 'c')
              EatChar(1150);
            else if (c == 'd')
              EatChar(1030);
            else if (c == 'e')
              EatChar(1040);
            else if (c == 'f')
              EatChar(1050);
            else if (c == 'g')
              EatChar(1065);
            else if (c == 'i')
              EatChar(1070);
            else if (c == 'l')
              EatChar(1080);
            else if (c == 'n')
              EatChar(1090);
            else if (c == 'o')
              EatChar(1100);
            else if (c == 'r')
              EatChar(1110);
            else if (c == 't')
              EatChar(1120);
            else if (c == 'u')
              EatChar(1130);
            else if (c == 'w')
              EatChar(1140);
            else if (Char.IsLetter(c) || c == '_')
              EatChar(1000);
            else
              return EatCharAndCreateToken(0, LuaToken.InvalidChar);
            break;
          #endregion
          #region -- 10 Whitespaces ---------------------------------------------------
          case 10:
            if (c == '\n' || c == '\0' || !Char.IsWhiteSpace(c))
              return CreateToken(0, LuaToken.Whitespace);
            else
              NextChar(10);
            break;
          #endregion
          #region -- 20 ---------------------------------------------------------------
          case 20:
            if (c == '=')
              return NextCharAndCreateToken(0, LuaToken.Equal);
            else
              return CreateToken(0, LuaToken.Assign);
          case 21:
            if (c == '=')
              return NextCharAndCreateToken(0, LuaToken.NotEqual);
            else
              return CreateToken(0, LuaToken.InvalidChar);
          case 22:
            if (c == '=')
              return NextCharAndCreateToken(0, LuaToken.LowerEqual);
            else
              return CreateToken(0, LuaToken.Lower);
          case 23:
            if (c == '=')
              return NextCharAndCreateToken(0, LuaToken.GreaterEqual);
            else
              return CreateToken(0, LuaToken.Greater);
          case 24:
            if (c == '.')
              NextChar(25);
            else
              return CreateToken(0, LuaToken.Dot);
            break;
          case 25:
            if (c == '.')
              NextChar(26);
            else
              return CreateToken(0, LuaToken.DotDot);
            break;
          case 26:
            if (c == '.')
              return NextCharAndCreateToken(0, LuaToken.DotDotDot);
            else
              return CreateToken(0, LuaToken.DotDotDot);
          case 27:
            if (c == '=' || c == '[')
              return ReadTextBlock(true);
            else
              return CreateToken(0, LuaToken.BracketSquareOpen);
          #endregion
          #region -- 30 Label ---------------------------------------------------------
          case 30:
            if (c == ':')
              NextChar(31);
            else
              return CreateToken(0, LuaToken.Colon);
            break;
          case 31:
            if (c == ':')
              return NextCharAndCreateToken(0, LuaToken.ColonColon);
            else
              return CreateToken(0, LuaToken.ColonColon);
          #endregion
          #region -- 40 String --------------------------------------------------------
          case 40:
            if (c == cStringMode)
              return NextCharAndCreateToken(0, LuaToken.String);
            else if (c == '\\')
              NextChar(41);
            else if (c == '\0' || c == '\n')
              return CreateToken(0, LuaToken.InvalidString);
            else
              EatChar(40);
            break;
          case 41:
            if (c == 'a') { AppendValue('\a'); NextChar(40); }
            else if (c == 'b') { AppendValue('\b'); NextChar(40); }
            else if (c == 'f') { AppendValue('\f'); NextChar(40); }
            else if (c == 'n') { AppendValue('\n'); NextChar(40); }
            else if (c == 'r') { AppendValue('\r'); NextChar(40); }
            else if (c == 't') { AppendValue('\t'); NextChar(40); }
            else if (c == 'v') { AppendValue('\v'); NextChar(40); }
            else if (c == '\\') { AppendValue('\\'); NextChar(40); }
            else if (c == '"') { AppendValue('"'); NextChar(40); }
            else if (c == '\'') { AppendValue('\''); NextChar(40); }
            else if (c == 'x')
              NextChar(45);
            else if (c == 'z')
              NextChar(48);

            else if (c >= '0' && c <= '9')
            {
              bChar = unchecked((byte)(c - '0'));
              NextChar(42);
            }
            else
              EatChar(40);
            break;
          case 42:
            if (c >= '0' && c <= '9')
            {
              bChar = unchecked((byte)(bChar * 10 + (c - '0')));
              NextChar(43);
            }
            else
            {
              AppendValue((char)bChar);
              goto case 40;
            }
            break;
          case 43:
            if (c >= '0' && c <= '9')
            {
              bChar = unchecked((byte)(bChar * 10 + (c - '0')));
              AppendValue((char)bChar);
              NextChar(40);
            }
            else
            {
              AppendValue((char)bChar);
              goto case 40;
            }
            break;
          case 45:
            if (c >= '0' && c <= '9')
            {
              bChar = unchecked((byte)(c - '0'));
              NextChar(46);
            }
            else if (c >= 'a' && c <= 'f')
            {
              bChar = unchecked((byte)(c - 'a' + 10));
              NextChar(46);
            }
            else if (c >= 'A' || c <= 'F')
            {
              bChar = unchecked((byte)(c - 'A' + 10));
              NextChar(46);
            }
            else
            {
              AppendValue('x');
              goto case 40;
            }
            break;
          case 46:
            if (c >= '0' && c <= '9')
            {
              bChar = unchecked((byte)((bChar << 4) +(c - '0')));
              AppendValue((char)bChar);
              NextChar(40);
            }
            else if (c >= 'a' && c <= 'f')
            {
              bChar = unchecked((byte)((bChar << 4) + (c - 'a' + 10)));
              AppendValue((char)bChar);
              NextChar(40);
            }
            else if (c >= 'A' || c <= 'F')
            {
              bChar = unchecked((byte)((bChar << 4) + (c - 'A' + 10)));
              AppendValue((char)bChar);
              NextChar(40);
            }
            else
            {
              AppendValue((char)bChar);
              goto case 40;
            }
            break;
          case 48:
            if (Char.IsWhiteSpace(c))
              NextChar(48);
            else
              goto case 40;
            break;
          #endregion
          #region -- 50 Kommentar -----------------------------------------------------
          case 50:
            if (c == '-') // Kommentar
              NextChar(51);
            else
              return CreateToken(0, LuaToken.Minus);
            break;
          case 51:
            if (c == '[')
            {
              NextChar(51);
              return ReadTextBlock(false);
            }
            else if (c == '\n')
              return NextCharAndCreateToken(0, LuaToken.Comment);
            else
              NextChar(52);
            break;
          case 52:
            if (c == '\0')
              return CreateToken(0, LuaToken.Comment);
            else if (c == '\n')
              return NextCharAndCreateToken(0, LuaToken.Comment);
            else
              NextChar(52);
            break;
          #endregion
          #region -- 60 Number --------------------------------------------------------
          case 60:
            if (c == 'x' || c == 'X')
              EatChar(70);
            else
              goto case 61;
            break;
          case 61:
            if (c == '.')
              EatChar(62);
            else if (c == 'e' || c == 'E')
              EatChar(63);
            else if (c >= '0' && c <= '9')
              EatChar(61);
            else
              return CreateToken(0, LuaToken.Number);
            break;
          case 62:
            if (c == 'e' || c == 'E')
              EatChar(63);
            else if (c >= '0' && c <= '9')
              EatChar(62);
            else
              return CreateToken(0, LuaToken.Number);
            break;
          case 63:
            if (c == '-' || c == '+')
              EatChar(64);
            else if (c >= '0' && c <= '9')
              EatChar(64);
            else
              return CreateToken(0, LuaToken.Number);
            break;
          case 64:
            if (c >= '0' && c <= '9')
              EatChar(64);
            else
              return CreateToken(0, LuaToken.Number);
            break;
          #endregion
          #region -- 70 HexNumber -----------------------------------------------------
          case 70:
            if (c == '.')
              EatChar(71);
            else if (c == 'p' || c == 'P')
              EatChar(72);
            else if (c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F')
              EatChar(70);
            else
              return CreateToken(0, LuaToken.HexNumber);
            break;
          case 71:
            if (c == 'p' || c == 'P')
              EatChar(72);
            else if (c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F')
              EatChar(71);
            else
              return CreateToken(0, LuaToken.HexNumber);
            break;
          case 72:
            if (c == '-' || c == '+')
              EatChar(73);
            else if (c >= '0' && c <= '9')
              EatChar(73);
            else
              return CreateToken(0, LuaToken.HexNumber);
            break;
          case 73:
            if (c >= '0' && c <= '9')
              EatChar(73);
            else
              return CreateToken(0, LuaToken.HexNumber);
            break;        
          #endregion
          #region -- 1000 Ident or Keyword --------------------------------------------
          case 1000:
            if (c == '_' || Char.IsLetterOrDigit(c))
              EatChar(1000);
            else
              return CreateToken(0, LuaToken.Identifier);
            break;
          // and
          case 1010: if (c == 'n') EatChar(1011); else goto case 1000; break;
          case 1011: if (c == 'd') EatChar(1012); else goto case 1000; break;
          case 1012: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwAnd); else goto case 1000;
          // break
          case 1020: if (c == 'r') EatChar(1021); else goto case 1000; break;
          case 1021: if (c == 'e') EatChar(1022); else goto case 1000; break;
          case 1022: if (c == 'a') EatChar(1023); else goto case 1000; break;
          case 1023: if (c == 'k') EatChar(1024); else goto case 1000; break;
          case 1024: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwBreak); else goto case 1000;
          // do
          case 1030: if (c == 'o') EatChar(1031); else goto case 1000; break;
          case 1031: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwDo); else goto case 1000;
          // else, elseif end
          case 1040: if (c == 'n') EatChar(1041); else if (c == 'l') EatChar(1043); else goto case 1000; break;
          case 1041: if (c == 'd') EatChar(1042); else goto case 1000; break;
          case 1042: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwEnd); else goto case 1000;
          case 1043: if (c == 's') EatChar(1044); else goto case 1000; break;
          case 1044: if (c == 'e') EatChar(1045); else goto case 1000; break;
          case 1045: if (c == 'i') EatChar(1046); else if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwElse); else goto case 1000; break;
          case 1046: if (c == 'f') EatChar(1047); else goto case 1000; break;
          case 1047: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwElseif); else goto case 1000;
          // false, for, function
          case 1050: if (c == 'a') EatChar(1051); else if (c == 'o') EatChar(1055); else if (c == 'u') EatChar(1057); else goto case 1000; break;
          case 1051: if (c == 'l') EatChar(1052); else goto case 1000; break;
          case 1052: if (c == 's') EatChar(1053); else goto case 1000; break;
          case 1053: if (c == 'e') EatChar(1054); else goto case 1000; break;
          case 1054: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwFalse); else goto case 1000;
          case 1055: if (c == 'r') EatChar(1056); else goto case 1000; break;
          case 1056: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwFor); else goto case 1000;
          case 1057: if (c == 'n') EatChar(1058); else goto case 1000; break;
          case 1058: if (c == 'c') EatChar(1059); else goto case 1000; break;
          case 1059: if (c == 't') EatChar(1060); else goto case 1000; break;
          case 1060: if (c == 'i') EatChar(1061); else goto case 1000; break;
          case 1061: if (c == 'o') EatChar(1062); else goto case 1000; break;
          case 1062: if (c == 'n') EatChar(1063); else goto case 1000; break;
          case 1063: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwFunction); else goto case 1000;
          // goto
          case 1065: if (c == 'o') EatChar(1066); else goto case 1000; break;
          case 1066: if (c == 't') EatChar(1067); else goto case 1000; break;
          case 1067: if (c == 'o') EatChar(1068); else goto case 1000; break;
          case 1068: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwGoto); else goto case 1000;
          // if, in
          case 1070: if (c == 'f') EatChar(1071); else if (c == 'n') EatChar(1072); else goto case 1000; break;
          case 1071: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwIf); else goto case 1000;
          case 1072: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwIn); else goto case 1000;
          // local
          case 1080: if (c == 'o') EatChar(1081); else goto case 1000; break;
          case 1081: if (c == 'c') EatChar(1082); else goto case 1000; break;
          case 1082: if (c == 'a') EatChar(1083); else goto case 1000; break;
          case 1083: if (c == 'l') EatChar(1084); else goto case 1000; break;
          case 1084: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwLocal); else goto case 1000;
          // nil, not
          case 1090: if (c == 'i') EatChar(1091); else if (c == 'o') EatChar(1093); else goto case 1000; break;
          case 1091: if (c == 'l') EatChar(1092); else goto case 1000; break;
          case 1092: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwNil); else goto case 1000;
          case 1093: if (c == 't') EatChar(1094); else goto case 1000; break;
          case 1094: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwNot); else goto case 1000;
          // or
          case 1100: if (c == 'r') EatChar(1101); else goto case 1000; break;
          case 1101: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwOr); else goto case 1000;
          // repeat, return
          case 1110: if (c == 'e') EatChar(1111); else goto case 1000; break;
          case 1111: if (c == 'p') EatChar(1112); else if (c == 't') EatChar(1116); else goto case 1000; break;
          case 1112: if (c == 'e') EatChar(1113); else goto case 1000; break;
          case 1113: if (c == 'a') EatChar(1114); else goto case 1000; break;
          case 1114: if (c == 't') EatChar(1115); else goto case 1000; break;
          case 1115: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwRepeat); else goto case 1000;
          case 1116: if (c == 'u') EatChar(1117); else goto case 1000; break;
          case 1117: if (c == 'r') EatChar(1118); else goto case 1000; break;
          case 1118: if (c == 'n') EatChar(1119); else goto case 1000; break;
          case 1119: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwReturn); else goto case 1000;
          // then, true
          case 1120: if (c == 'h') EatChar(1121); else if (c == 'r') EatChar(1124); else goto case 1000; break;
          case 1121: if (c == 'e') EatChar(1122); else goto case 1000; break;
          case 1122: if (c == 'n') EatChar(1123); else goto case 1000; break;
          case 1123: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwThen); else goto case 1000;
          case 1124: if (c == 'u') EatChar(1125); else goto case 1000; break;
          case 1125: if (c == 'e') EatChar(1126); else goto case 1000; break;
          case 1126: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwTrue); else goto case 1000;
          // until
          case 1130: if (c == 'n') EatChar(1131); else goto case 1000; break;
          case 1131: if (c == 't') EatChar(1132); else goto case 1000; break;
          case 1132: if (c == 'i') EatChar(1133); else goto case 1000; break;
          case 1133: if (c == 'l') EatChar(1134); else goto case 1000; break;
          case 1134: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwUntil); else goto case 1000;
          // while
          case 1140: if (c == 'h') EatChar(1141); else goto case 1000; break;
          case 1141: if (c == 'i') EatChar(1142); else goto case 1000; break;
          case 1142: if (c == 'l') EatChar(1143); else goto case 1000; break;
          case 1143: if (c == 'e') EatChar(1144); else goto case 1000; break;
          case 1144: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwWhile); else goto case 1000;
          // cast
          case 1150: if (c == 'a') EatChar(1151); else goto case 1000; break;
          case 1151: if (c == 's') EatChar(1152); else goto case 1000; break;
          case 1152: if (c == 't') EatChar(1153); else goto case 1000; break;
          case 1153: if (!Char.IsLetterOrDigit(c)) return CreateToken(0, LuaToken.KwCast); else goto case 1000;
          #endregion
        }
      }
    } // func NextToken

    private Token<LuaToken> ReadTextBlock(bool lStringMode)
    {
      int iSearch = 0;
      int iFind = 0;

      // Zähle die =
      while (Cur == '=')
      {
        NextChar(0);
        iSearch++;
      }
      if (Cur != '[')
        return NextCharAndCreateToken(0, lStringMode ? LuaToken.InvalidString : LuaToken.InvalidComment);
      NextChar(0);

      // Überspringe WhiteSpace bis zum ersten Zeilenumbruch
      while (Cur != '\0' && Char.IsWhiteSpace(Cur))
      {
        if (Cur == '\n')
        {
          NextChar(0);
          break;
        }
        else
          NextChar(0);
      }

      // Suche das Ende
    ReadChars:
      while (Cur != ']')
      {
        if (Cur == '\0')
          return NextCharAndCreateToken(0, lStringMode ? LuaToken.InvalidString : LuaToken.InvalidComment);
        else if (lStringMode)
          EatChar(0);
        else
          NextChar(0);
      }

      // Zähle die =
      iFind = 0;
      NextChar(0);
      while (Cur == '=')
      {
        NextChar(0);
        iFind++;
      }
      if (Cur == ']' && iFind == iSearch)
        return NextCharAndCreateToken(0, lStringMode ? LuaToken.String : LuaToken.Comment);
      else
      {
        AppendValue(']');
        for (int i = 0; i < iFind; i++)
          AppendValue('=');
        goto ReadChars;
      }
    } // proc ReadTextBlock

    #endregion
  } // class LuaLexer

  #endregion
}