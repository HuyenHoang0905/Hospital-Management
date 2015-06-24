using System;
using System.Drawing;

namespace DevComponents.Instrumentation
{
    public class Seg16Element : DigitalElement
    {
        #region Private variables

        private int _DiagSlant;
        private int _DiagWidth;

        #endregion

        public Seg16Element(NumericIndicator numIndicator)
            : base(numIndicator, 16, 6)
        {
        }

        #region RecalcSegments

        public override void RecalcSegments()
        {
            double d = (ElemWidth * NumIndicator.SegmentWidth) / 8;

            _DiagWidth = (int)(Math.Sqrt((d * d) * 2));
            _DiagWidth = Math.Max(1, _DiagWidth);

            d = Math.Sqrt(ElemHeight * ElemHeight + ElemWidth * ElemWidth) / 2;

            _DiagSlant = (int)((SegWidth  * d) / ElemWidthHalf);

            CalcSegment0();
            CalcSegment1();
            CalcSegment2();
            CalcSegment3();
            CalcSegment4();
            CalcSegment5();
            CalcSegment6();
            CalcSegment7();
            CalcSegment8();
            CalcSegment9();
            CalcSegment10();
            CalcSegment11();
            CalcSegment12();
            CalcSegment13();
            CalcSegment14();
            CalcSegment15();
        }

        #region CalcSegment0

        private void CalcSegment0()
        {
            Point[] pts = SegPoints[0];

            pts[0].X = SegWidth + 1; pts[0].Y = 0;
            pts[1].X = ElemWidthHalf - 1; pts[1].Y = 0;
            pts[2].X = ElemWidthHalf - 1; pts[2].Y = SegStep;
            pts[3].X = ElemWidthHalf - SegWidthHalf - 1; pts[3].Y = SegWidth;
            pts[4].X = SegWidth + 1; pts[4].Y = SegWidth;
            pts[5].X = SegWidthHalf + 1; pts[5].Y = SegWidthHalf;
        }

        #endregion

        #region CalcSegment1

        private void CalcSegment1()
        {
            Point[] pts = SegPoints[1];

            pts[0].X = ElemWidthHalf + 1; pts[0].Y = 0;
            pts[1].X = ElemWidth - SegWidth - 1; pts[1].Y = 0;
            pts[2].X = ElemWidth - SegWidthHalf - 1; pts[2].Y = SegWidthHalf;
            pts[3].X = ElemWidth - SegWidth - 1; pts[3].Y = SegWidth;
            pts[4].X = ElemWidthHalf + SegWidthHalf + 1; pts[4].Y = SegWidth;
            pts[5].X = ElemWidthHalf + 1; pts[5].Y = SegStep;
        }

        #endregion

        #region CalcSegment2

        private void CalcSegment2()
        {
            Point[] pts = SegPoints[2];

            pts[0].X = 0; pts[0].Y = SegWidth + 1;
            pts[1].X = SegWidthHalf; pts[1].Y = SegWidthHalf + 1;
            pts[2].X = SegWidth; pts[2].Y = pts[0].Y;
            pts[3].X = pts[2].X; pts[3].Y = ElemHeightHalf - SegWidthHalf - 1;
            pts[4].X = SegStep; pts[4].Y = ElemHeightHalf - 1;
            pts[5].X = 0; pts[5].Y = pts[4].Y;
        }

        #endregion

        #region CalcSegment3

        private void CalcSegment3()
        {
            Point[] pts = SegPoints[3];

            pts[0].X = SegWidth + 1; pts[0].Y = pts[0].X;
            pts[1].X = pts[0].X + _DiagWidth; pts[1].Y = pts[0].Y;
            pts[2].X = ElemWidthHalf - SegWidthHalf - 1; pts[2].Y = ElemHeightHalf - SegWidthHalf - _DiagSlant;
            pts[3].X = pts[2].X; pts[3].Y = pts[2].Y + _DiagSlant;
            pts[4].X = pts[0].X; pts[4].Y = pts[0].Y + _DiagWidth;
            pts[5].X = pts[0].X; pts[5].Y = pts[0].Y;
        }

        #endregion

        #region CalcSegment4

        private void CalcSegment4()
        {
            Point[] pts = SegPoints[4];

            pts[0].X = ElemWidthHalf - SegWidthHalf; pts[0].Y = SegWidth + 1;
            pts[1].X = ElemWidthHalf; pts[1].Y = SegStep + 1;
            pts[2].X = ElemWidthHalf + SegWidthHalf; pts[2].Y = pts[0].Y;
            pts[3].X = pts[2].X; pts[3].Y = ElemHeightHalf - SegWidthHalf - 1;
            pts[4].X = pts[1].X; pts[4].Y = ElemHeightHalf - 1;
            pts[5].X = pts[0].X; pts[5].Y = pts[3].Y;
        }

        #endregion

        #region CalcSegment5

        private void CalcSegment5()
        {
            Point[] pts = SegPoints[5];

            pts[0].X = ElemWidth - SegWidth - _DiagWidth - 1; pts[0].Y = SegWidth + 1;
            pts[1].X = pts[0].X + _DiagWidth; pts[1].Y = pts[0].Y;
            pts[2].X = pts[1].X; pts[2].Y = pts[1].Y + _DiagWidth;
            pts[3].X = ElemWidthHalf + SegWidthHalf + 1; pts[3].Y = ElemHeightHalf - SegWidthHalf;
            pts[4].X = pts[3].X; pts[4].Y = pts[3].Y - _DiagSlant;
            pts[5].X = pts[0].X; pts[5].Y = pts[0].Y;
        }

        #endregion

        #region CalcSegment6

        private void CalcSegment6()
        {
            Point[] pts = SegPoints[6];

            pts[0].X = ElemWidth - SegWidth; pts[0].Y = SegWidth + 1;
            pts[1].X = pts[0].X + SegWidthHalf; pts[1].Y = SegWidthHalf + 1;
            pts[2].X = ElemWidth; pts[2].Y = pts[0].Y;
            pts[3].X = pts[2].X; pts[3].Y = ElemHeightHalf - 1;
            pts[4].X = pts[3].X - SegStep; pts[4].Y = pts[3].Y;
            pts[5].X = ElemWidth - SegWidth; pts[5].Y = pts[3].Y - SegWidthHalf;
        }

        #endregion

        #region CalcSegment7

        private void CalcSegment7()
        {
            Point[] pts = SegPoints[7];

            pts[0].X = SegWidth + 1; pts[0].Y = ElemHeightHalf - SegWidthHalf;
            pts[1].X = ElemWidthHalf - SegWidthHalf - 1; pts[1].Y = pts[0].Y;
            pts[2].X = ElemWidthHalf - 1; pts[2].Y = ElemHeightHalf;
            pts[3].X = pts[1].X; pts[3].Y = ElemHeightHalf + SegWidthHalf;
            pts[4].X = pts[0].X; pts[4].Y = pts[3].Y;
            pts[5].X = SegStep + 1; pts[5].Y = pts[2].Y;
        }

        #endregion

        #region CalcSegment8

        private void CalcSegment8()
        {
            Point[] pts = SegPoints[8];

            pts[0].X = ElemWidthHalf + SegWidthHalf + 1; pts[0].Y = ElemHeightHalf - SegWidthHalf;
            pts[1].X = ElemWidth - SegWidth - 1; pts[1].Y = pts[0].Y;
            pts[2].X = ElemWidth - SegStep - 1; pts[2].Y = ElemHeightHalf;
            pts[3].X = pts[1].X; pts[3].Y = ElemHeightHalf + SegWidthHalf;
            pts[4].X = pts[0].X; pts[4].Y = pts[3].Y;
            pts[5].X = ElemWidthHalf + 1; pts[5].Y = pts[2].Y;
        }

        #endregion

        #region CalcSegment9

        private void CalcSegment9()
        {
            Point[] pts = SegPoints[9];

            pts[0].X = 0; pts[0].Y = ElemHeightHalf + 1;
            pts[1].X = SegStep; pts[1].Y = pts[0].Y;
            pts[2].X = SegWidth; pts[2].Y = pts[1].Y + SegWidthHalf;
            pts[3].X = pts[2].X; pts[3].Y = ElemHeight - SegWidth - 1;
            pts[4].X = SegWidthHalf; pts[4].Y = ElemHeight - SegWidthHalf - 1;
            pts[5].X = 0; pts[5].Y = pts[3].Y;
        }

        #endregion

        #region CalcSegment10

        private void CalcSegment10()
        {
            Point[] pts = SegPoints[10];

            pts[0].X = ElemWidthHalf - SegWidthHalf - 1; pts[0].Y = ElemHeightHalf + SegWidthHalf;
            pts[1].X = pts[0].X; pts[1].Y = pts[0].Y + _DiagSlant;
            pts[2].X = SegWidth + _DiagWidth + 1; pts[2].Y = ElemHeight - SegWidth - 1;
            pts[3].X = SegWidth + 1; pts[3].Y = ElemHeight - SegWidth - 1;
            pts[4].X = pts[3].X; pts[4].Y = pts[3].Y - _DiagWidth;
            pts[5].X = pts[0].X; pts[5].Y = pts[0].Y;
        }

        #endregion

        #region CalcSegment11

        private void CalcSegment11()
        {
            Point[] pts = SegPoints[11];

            pts[0].X = ElemWidthHalf - SegWidthHalf; pts[0].Y = ElemHeightHalf + SegWidthHalf + 1;
            pts[1].X = ElemWidthHalf; pts[1].Y = ElemHeightHalf + 1;
            pts[2].X = ElemWidthHalf + SegWidthHalf; pts[2].Y = pts[0].Y;
            pts[3].X = pts[2].X; pts[3].Y = ElemHeight - SegWidth - 1;
            pts[4].X = pts[1].X; pts[4].Y = ElemHeight - SegStep - 1;
            pts[5].X = pts[0].X; pts[5].Y = pts[3].Y;
        }

        #endregion

        #region CalcSegment12

        private void CalcSegment12()
        {
            Point[] pts = SegPoints[12];

            pts[0].X = ElemWidthHalf + SegWidthHalf + 1; pts[0].Y = ElemHeightHalf + SegWidthHalf;
            pts[1].X = ElemWidth - SegWidth - 1; pts[1].Y = ElemHeight - SegWidth - _DiagWidth - 1;
            pts[2].X = pts[1].X; pts[2].Y = pts[1].Y + _DiagWidth;
            pts[3].X = pts[2].X - _DiagWidth; pts[3].Y = pts[2].Y;
            pts[4].X = pts[0].X; pts[4].Y = pts[0].Y + _DiagSlant;
            pts[5].X = pts[0].X; pts[5].Y = pts[0].Y;
        }

        #endregion

        #region CalcSegment13

        private void CalcSegment13()
        {
            Point[] pts = SegPoints[13];

            pts[0].X = ElemWidth - SegStep; pts[0].Y = ElemHeightHalf + 1;
            pts[1].X = ElemWidth; pts[1].Y = pts[0].Y;
            pts[2].X = pts[1].X; pts[2].Y = ElemHeight - SegWidth - 1;
            pts[3].X = ElemWidth - SegWidthHalf; pts[3].Y = ElemHeight - SegWidthHalf - 1;
            pts[4].X = ElemWidth - SegWidth; pts[4].Y = pts[2].Y;
            pts[5].X = pts[4].X; pts[5].Y = ElemHeightHalf + SegWidthHalf + 1;
        }

        #endregion

        #region CalcSegment14

        private void CalcSegment14()
        {
            Point[] pts = SegPoints[14];

            pts[0].X = SegWidth + 1; pts[0].Y = ElemHeight - SegWidth;
            pts[1].X = ElemWidthHalf - SegWidthHalf - 1; pts[1].Y = pts[0].Y;
            pts[2].X = ElemWidthHalf - 1; pts[2].Y = ElemHeight - SegStep;
            pts[3].X = pts[2].X; pts[3].Y = ElemHeight;
            pts[4].X = pts[0].X; pts[4].Y = pts[3].Y;
            pts[5].X = SegWidthHalf + 1; pts[5].Y = ElemHeight - SegWidthHalf;
        }

        #endregion

        #region CalcSegment15

        private void CalcSegment15()
        {
            Point[] pts = SegPoints[15];

            pts[0].X = ElemWidthHalf + SegWidthHalf + 1; pts[0].Y = ElemHeight - SegWidth;
            pts[1].X = ElemWidth - SegWidth - 1; pts[1].Y = pts[0].Y;
            pts[2].X = ElemWidth - SegWidthHalf - 1; pts[2].Y = ElemHeight - SegWidthHalf;
            pts[3].X = pts[1].X; pts[3].Y = ElemHeight;
            pts[4].X = ElemWidthHalf + 1; pts[4].Y = ElemHeight;
            pts[5].X = pts[4].X; pts[5].Y = ElemHeight - SegStep;
        }

        #endregion

        #endregion

        #region GetDigitSegments

        public override int GetDigitSegments(char digit)
        {
            switch (digit)
            {
                #region Numeric

                case '0': return ((int)Seg16Segments.Zero);
                case '1': return ((int)Seg16Segments.One);
                case '2': return ((int)Seg16Segments.Two);
                case '3': return ((int)Seg16Segments.Three);
                case '4': return ((int)Seg16Segments.Four);
                case '5': return ((int)Seg16Segments.Five);
                case '6': return ((int)Seg16Segments.Six);
                case '7': return ((int)Seg16Segments.Seven);
                case '8': return ((int)Seg16Segments.Eight);
                case '9': return ((int)Seg16Segments.Nine);

                #endregion

                #region Lowercase Alpha

                case 'a': return ((int)Seg16Segments.a);
                case 'b': return ((int)Seg16Segments.b);
                case 'c': return ((int)Seg16Segments.c);
                case 'd': return ((int)Seg16Segments.d);
                case 'e': return ((int)Seg16Segments.e);
                case 'f': return ((int)Seg16Segments.f);
                case 'g': return ((int)Seg16Segments.g);
                case 'h': return ((int)Seg16Segments.h);
                case 'i': return ((int)Seg16Segments.i);
                case 'j': return ((int)Seg16Segments.j);
                case 'k': return ((int)Seg16Segments.k);
                case 'l': return ((int)Seg16Segments.l);
                case 'm': return ((int)Seg16Segments.m);
                case 'n': return ((int)Seg16Segments.n);
                case 'o': return ((int)Seg16Segments.o);
                case 'p': return ((int)Seg16Segments.p);
                case 'q': return ((int)Seg16Segments.q);
                case 'r': return ((int)Seg16Segments.r);
                case 's': return ((int)Seg16Segments.s);
                case 't': return ((int)Seg16Segments.t);
                case 'u': return ((int)Seg16Segments.u);
                case 'v': return ((int)Seg16Segments.v);
                case 'w': return ((int)Seg16Segments.w);
                case 'x': return ((int)Seg16Segments.x);
                case 'y': return ((int)Seg16Segments.y);
                case 'z': return ((int)Seg16Segments.z);

                #endregion

                #region Uppercase alpha

                case 'A': return ((int)Seg16Segments.A);
                case 'B': return ((int)Seg16Segments.B);
                case 'C': return ((int)Seg16Segments.C);
                case 'D': return ((int)Seg16Segments.D);
                case 'E': return ((int)Seg16Segments.E);
                case 'F': return ((int)Seg16Segments.F);
                case 'G': return ((int)Seg16Segments.G);
                case 'H': return ((int)Seg16Segments.H);
                case 'I': return ((int)Seg16Segments.I);
                case 'J': return ((int)Seg16Segments.J);
                case 'K': return ((int)Seg16Segments.K);
                case 'L': return ((int)Seg16Segments.L);
                case 'M': return ((int)Seg16Segments.M);
                case 'N': return ((int)Seg16Segments.N);
                case 'O': return ((int)Seg16Segments.O);
                case 'P': return ((int)Seg16Segments.P);
                case 'Q': return ((int)Seg16Segments.Q);
                case 'R': return ((int)Seg16Segments.R);
                case 'S': return ((int)Seg16Segments.S);
                case 'T': return ((int)Seg16Segments.T);
                case 'U': return ((int)Seg16Segments.U);
                case 'V': return ((int)Seg16Segments.V);
                case 'W': return ((int)Seg16Segments.W);
                case 'X': return ((int)Seg16Segments.X);
                case 'Y': return ((int)Seg16Segments.Y);
                case 'Z': return ((int)Seg16Segments.Z);

                #endregion

                #region Extended chars

                case ' ': return ((int)Seg16Segments.Space);
                case '!': return ((int)Seg16Segments.ExPoint);
                case '"': return ((int)Seg16Segments.DQuote);
                case '#': return ((int)Seg16Segments.PoundSign);
                case '$': return ((int)Seg16Segments.DollarSign);
                case '%': return ((int)Seg16Segments.PercentSign);
                case '&': return ((int)Seg16Segments.Ampersand);
                case '\'': return ((int)Seg16Segments.SQuote);
                case '(': return ((int)Seg16Segments.LParen);
                case ')': return ((int)Seg16Segments.RParen);
                case '*': return ((int)Seg16Segments.Asterisk);
                case '+': return ((int)Seg16Segments.PlusSign);
                case ',': return ((int)Seg16Segments.Comma);
                case '-': return ((int)Seg16Segments.Dash);
                case '/': return ((int)Seg16Segments.FSlash);
                case ':': return ((int)Seg16Segments.Colon);
                case ';': return ((int)Seg16Segments.Semicolon);
                case '<': return ((int)Seg16Segments.LessThan);
                case '=': return ((int)Seg16Segments.Equals);
                case '>': return ((int)Seg16Segments.GreaterThan);
                case '?': return ((int)Seg16Segments.QuestionMark);
                case '@': return ((int)Seg16Segments.Atsign);
                case '[': return ((int)Seg16Segments.LBracket);
                case '\\': return ((int)Seg16Segments.BackSlash);
                case ']': return ((int)Seg16Segments.RBracket);
                case '^': return ((int)Seg16Segments.Caret);
                case '_': return ((int)Seg16Segments.Underline);
                case '`': return ((int)Seg16Segments.Apostrophe);
                case '{': return ((int)Seg16Segments.LBrace);
                case '|': return ((int)Seg16Segments.Pipe);
                case '}': return ((int)Seg16Segments.RBrace);
                case '~': return ((int)Seg16Segments.Tilde);
                case '¤': return ((int)Seg16Segments.Pound);
                case '°': return ((int)Seg16Segments.Degree);
                case '±': return ((int)Seg16Segments.PlusMinus);

                #endregion

                default: return ((int)Seg16Segments.None);
            }
        }

        #endregion
    }

    #region Enums

    #region Seg16Segments

    // ReSharper disable InconsistentNaming

    public enum Seg16Segments : int
    {
        None = 0,

        #region Numeric

        Zero = 0xE667,
        One = 0x810,
        Two = 0xC3C3,
        Three = 0xE1C3,
        Four = 0x21C4,
        Five = 0xE187,
        Six = 0xE387,
        Seven = 0x2043,
        Eight = 0xE3C7,
        Nine = 0xE1C7,

        #endregion

        #region Lowercase alpha

        a = 0xCA80,
        b = 0xA910,
        c = 0x8900,
        d = 0xA940,
        e = 0x4680,
        f = 0x912,
        g = 0xB100,
        h = 0x2910,
        i = 0x8820,
        j = 0xA840,
        k = 0x1830,
        l = 0x8810,
        m = 0x2B80,
        n = 0x2900,
        o = 0xA900,
        p = 0x28C,
        q = 0x8895,
        r = 0x900,
        s = 0x9100,
        t = 0x8990,
        u = 0xA800,
        v = 0x3000,
        w = 0x3600,
        x = 0x1580,
        y = 0xB000,
        z = 0x4480,

        #endregion

        #region Uppercase alpha

        A = 0x23C7,
        B = 0xE953,
        C = 0xC207,
        D = 0xE853,
        E = 0xC387,
        F = 0x387,
        G = 0xE307,
        H = 0x23C4,
        I = 0xC813,
        J = 0xE240,
        K = 0x12A4,
        L = 0xC204,
        M = 0x226C,
        N = 0x324C,
        O = 0xE247,
        P = 0x3C7,
        Q = 0xF247,
        R = 0x13C7,
        S = 0xE187,
        T = 0x813,
        U = 0xE244,
        V = 0x3048,
        W = 0xEA44,
        X = 0x1428,
        Y = 0x828,
        Z = 0xC423,

        #endregion

        #region Extended chars

        Space = 0x00,
        ExPoint = 0x8152,
        DQuote = 0x50,
        PoundSign = 0xE9D0,
        DollarSign = 0xE997,
        PercentSign = 0xADB5,
        Ampersand = 0xD299,
        SQuote = 0x10,
        LParen = 0x4205,
        RParen = 0xA042,
        Asterisk = 0x1DB8,
        PlusSign = 0x990,
        Comma = 0x400,
        Dash = 0x180,
        FSlash = 0x420,
        Colon = 0x810,
        Semicolon = 0x410,
        LessThan = 0x1020,
        Equals = 0xC180,
        GreaterThan = 0x408,
        QuestionMark = 0x943,
        Atsign = 0xAB47,
        LBracket = 0x4205,
        BackSlash = 0x1008,
        RBracket = 0xA042,
        Caret = 0x1400,
        Underline = 0xC000,
        Apostrophe = 0x8,
        LBrace = 0x8892,
        Pipe = 0x810,
        RBrace = 0x4911,
        Tilde = 0x2C,
        Pound = 0xAB14,
        Degree = 0x152,
        PlusMinus = 0xC990,

        #endregion
    }

    // ReSharper restore InconsistentNaming

    #endregion

    #endregion
}
