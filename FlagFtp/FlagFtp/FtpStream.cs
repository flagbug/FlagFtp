using System;
using System.IO;
using System.Runtime.Remoting;

namespace FlagFtp
{
    /// <summary>
    /// Represents a stream of a file from an FTP server which supports the read of the length property
    /// </summary>
    public class FtpStream : Stream
    {
        private readonly Stream internalStream;
        private readonly long length;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns>true if the stream supports reading; otherwise, false.
        ///   </returns>
        public override bool CanRead
        {
            get { return this.internalStream.CanRead; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>true if the stream supports seeking; otherwise, false.
        ///   </returns>
        public override bool CanSeek
        {
            get { return this.internalStream.CanSeek; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns>true if the stream supports writing; otherwise, false.
        ///   </returns>
        public override bool CanWrite
        {
            get { return this.internalStream.CanWrite; }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        public override void Flush()
        {
            this.internalStream.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <returns>
        /// A long value representing the length of the stream in bytes.
        ///   </returns>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// A class derived from Stream does not support seeking.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override long Length
        {
            get { return this.length; }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <returns>
        /// The current position within the stream.
        ///   </returns>
        ///
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support seeking.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override long Position
        {
            get { return this.internalStream.Position; }
            set { this.internalStream.Position = value; }
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset"/> or <paramref name="count"/> is negative.
        ///   </exception>
        ///
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support reading.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.internalStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support seeking, such as if the stream is constructed from a pipe or console output.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.internalStream.Seek(offset, origin);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override void SetLength(long value)
        {
            this.internalStream.SetLength(value);
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset"/> or <paramref name="count"/> is negative.
        ///   </exception>
        ///
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support writing.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.internalStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Begins an asynchronous read operation.
        /// </summary>
        /// <param name="buffer">The buffer to read the data into.</param>
        /// <param name="offset">The byte offset in <paramref name="buffer"/> at which to begin writing data read from the stream.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"/> that represents the asynchronous read, which could still be pending.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">
        /// Attempted an asynchronous read past the end of the stream, or a disk error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentException">
        /// One or more of the arguments is invalid.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The current Stream implementation does not support the read operation.
        ///   </exception>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.internalStream.BeginRead(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Begins an asynchronous write operation.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The byte offset in <paramref name="buffer"/> from which to begin writing.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the write is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
        /// <returns>
        /// An IAsyncResult that represents the asynchronous write, which could still be pending.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">
        /// Attempted an asynchronous write past the end of the stream, or a disk error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentException">
        /// One or more of the arguments is invalid.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The current Stream implementation does not support the write operation.
        ///   </exception>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.internalStream.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Gets a value that determines whether the current stream can time out.
        /// </summary>
        /// <returns>
        /// A value that determines whether the current stream can time out.
        ///   </returns>
        public override bool CanTimeout
        {
            get { return this.internalStream.CanTimeout; }
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        public override void Close()
        {
            this.internalStream.Close();
            base.Close();
        }

        /// <summary>
        /// Creates an object that contains all the relevant information required to generate a proxy used to communicate with a remote object.
        /// </summary>
        /// <param name="requestedType">The <see cref="T:System.Type"/> of the object that the new <see cref="T:System.Runtime.Remoting.ObjRef"/> will reference.</param>
        /// <returns>
        /// Information required to generate a proxy.
        /// </returns>
        /// <exception cref="T:System.Runtime.Remoting.RemotingException">
        /// This instance is not a valid remoting object.
        ///   </exception>
        ///
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        ///   </exception>
        ///
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
        ///   </PermissionSet>
        public override ObjRef CreateObjRef(Type requestedType)
        {
            return this.internalStream.CreateObjRef(requestedType);
        }

        /// <summary>
        /// Waits for the pending asynchronous read to complete.
        /// </summary>
        /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
        /// <returns>
        /// The number of bytes read from the stream, between zero (0) and the number of bytes you requested. Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is available.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="asyncResult"/> is null.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="asyncResult"/> did not originate from a <see cref="M:System.IO.Stream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)"/> method on the current stream.
        ///   </exception>
        ///
        /// <exception cref="T:System.IO.IOException">
        /// The stream is closed or an internal error has occurred.
        ///   </exception>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return this.internalStream.EndRead(asyncResult);
        }

        /// <summary>
        /// Ends an asynchronous write operation.
        /// </summary>
        /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="asyncResult"/> is null.
        ///   </exception>
        ///
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="asyncResult"/> did not originate from a <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)"/> method on the current stream.
        ///   </exception>
        ///
        /// <exception cref="T:System.IO.IOException">
        /// The stream is closed or an internal error has occurred.
        ///   </exception>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.internalStream.EndWrite(asyncResult);
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        ///   </exception>
        ///
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        ///   </PermissionSet>
        public override object InitializeLifetimeService()
        {
            return this.internalStream.InitializeLifetimeService();
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        /// <returns>
        /// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support reading.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override int ReadByte()
        {
            return this.internalStream.ReadByte();
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to read before timing out.
        /// </summary>
        /// <returns>
        /// A value, in miliseconds, that determines how long the stream will attempt to read before timing out.
        ///   </returns>
        ///
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:System.IO.Stream.ReadTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>.
        ///   </exception>
        public override int ReadTimeout
        {
            get
            {
                return this.internalStream.ReadTimeout;
            }
            set
            {
                this.internalStream.ReadTimeout = value;
            }
        }

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support writing, or the stream is already closed.
        ///   </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override void WriteByte(byte value)
        {
            this.internalStream.WriteByte(value);
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to write before timing out.
        /// </summary>
        /// <returns>
        /// A value, in miliseconds, that determines how long the stream will attempt to write before timing out.
        ///   </returns>
        ///
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:System.IO.Stream.WriteTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>.
        ///   </exception>
        public override int WriteTimeout
        {
            get { return this.internalStream.WriteTimeout; }
            set { this.internalStream.WriteTimeout = value; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            this.internalStream.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpStream"/> class.
        /// </summary>
        /// <param name="baseStream">The base stream.</param>
        /// <param name="length">The length of teh stream.</param>
        internal FtpStream(Stream baseStream, long length)
        {
            if (baseStream == null)
                throw new ArgumentNullException("baseStream");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            this.internalStream = baseStream;
            this.length = length;
        }
    }
}