﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGL.Objects.VertexBuffers
{
    /// <summary>
    /// 一个vertex array object。
    /// </summary>
    public class VertexArrayObject : IDisposable
    {
        BufferRenderer[] bufferRenderers;

        /// <summary>
        /// 一个vertex array object。
        /// </summary>
        /// <param name="propertyBuffers">给出此VAO要管理的所有VBO。</param>
        public VertexArrayObject(params BufferRenderer[] propertyBuffers)
        {
            this.bufferRenderers = propertyBuffers;
        }

        private bool disposedValue;

        /// <summary>
        /// 此VAO的ID，由OpenGL给出。
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// 在OpenGL中创建VAO。
        /// </summary>
        /// <param name="e"></param>
        /// <param name="shaderProgram"></param>
        public void Create(RenderEventArgs e, Shaders.ShaderProgram shaderProgram)
        {
            uint[] buffers = new uint[1];
            GL.GenVertexArrays(1, buffers);

            this.ID = buffers[0];

            this.Bind();
            foreach (var item in this.bufferRenderers)
            {
                item.Render(e, shaderProgram);
            }
            this.Unbind();
        }

        private void Bind()
        {
            GL.BindVertexArray(this.ID);
        }

        private void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Render(RenderEventArgs e, Shaders.ShaderProgram shaderProgram)
        {
            this.Bind();
            foreach (var item in this.bufferRenderers)
            {
                item.Render(e, shaderProgram);
            }
            this.Unbind();
        }

        public override string ToString()
        {
            return string.Format("ID: {0}", this.ID);
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~VertexArrayObject()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (this.disposedValue == false)
            {
                if (disposing)
                {
                    // Dispose managed resources.

                }

                // Dispose unmanaged resources.
                foreach (var item in this.bufferRenderers)
                {
                    item.Dispose();
                }
                GL.DeleteVertexArrays(1, new uint[] { this.ID });
            }

            this.disposedValue = true;
        }

    }
}
