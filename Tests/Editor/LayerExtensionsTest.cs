// MIT License
//
// Copyright (c) 2022 - 2023 Alexander Pluzhnikov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using NUnit.Framework;
using UnityEngine;

namespace UnityTagsGenerator.Tests.Editor
{
    public class LayerExtensionsTest
    {
        [Test]
        public void LayerExtensions_AllLayersTest()
        {
            var all = LayerExtensions.All();
            Assert.AreEqual(-1, all.value);
        }
        
        [Test]
        public void LayerExtensions_ExceptLayerNumbersTest()
        {
            var except = LayerExtensions.Except(1);
            Assert.AreEqual(-3, except.value);
        }
        
        [Test]
        public void LayerExtensions_ExceptLayerMaskTest()
        {
            var intMask = 1 << 1;
            var except = LayerExtensions.Except((LayerMask)intMask);
            Assert.AreEqual(-3, except.value);
        }
        
        [Test]
        public void LayerExtensions_WhereTest()
        {
            // mask value: 10
            var where = LayerExtensions.Where(1, 3);
            Assert.AreEqual(10, where.value);
        }
        
        [Test]
        public void LayerExtensions_UnionLayerMaskTest()
        {
            // mask value: 3
            var mask1 = LayerExtensions.Where(0, 1);
            // mask value: 24
            var mask2 = LayerExtensions.Where(3, 4);
            var union = mask1.Union(mask2);
            Assert.AreEqual(27, union.value);
        }
        
        [Test]
        public void LayerExtensions_UnionLayerNumbersTest()
        {
            // mask value: 3
            var mask1 = LayerExtensions.Where(0, 1);
            // mask value: 24
            var union = mask1.Union(3, 4);
            Assert.AreEqual(27, union.value);
        }
    }
}