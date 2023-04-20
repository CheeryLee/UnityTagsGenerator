﻿// MIT License
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

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityTagsGenerator
{
    public static class LayerExtensions
    {
        private const int LayersCount = 32;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerMask All()
        {
            var mask = 0;
            for (var i = 0; i < LayersCount; i++)
                mask |= 1 << i;

            return mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerMask Except(params int[] layerNumbers)
        {
            var mask = (int)All();
            foreach (var layer in layerNumbers)
                mask ^= layer;

            return mask;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerMask Except(LayerMask layerMask)
        {
            var mask = (int)All();
            return mask ^ layerMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerMask Where(params int[] layerNumbers)
        {
            var mask = 0;
            foreach (var layer in layerNumbers)
                mask |= 1 << layer;

            return mask;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerMask Union(LayerMask mask1, LayerMask mask2)
        {
            return mask1 | mask2;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerMask Union(LayerMask mask1, params int[] layerNumbers)
        {
            return Union(mask1, Where(layerNumbers));
        }
    }
}