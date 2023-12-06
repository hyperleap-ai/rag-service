﻿// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.KernelMemory;

// Note: this class is designed to avoid using Asp.Net IForm
// and avoiding dependencies on Asp.Net HTTP that would lead
// to dependency issues mixing .NET7 and .NET Standard 2.0
public class DocumentUploadRequest
{
    public class UploadedFile
    {
        /// <summary>
        /// Name of the file, without path.
        /// Note: the file name can be useful for RAG, so it's better to persist original file names to provide context to LLMs.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File content
        /// </summary>
        public Stream FileContent { get; set; } = Stream.Null;

        public UploadedFile()
        {
        }

        public UploadedFile(string fileName, Stream fileContent)
        {
            this.FileName = fileName;
            this.FileContent = fileContent;
        }
    }

    /// <summary>
    /// Name of the index where to store memories generated by the files uploaded.
    /// </summary>
    public string Index { get; set; } = string.Empty;

    /// <summary>
    /// Unique ID used for import pipeline and document ID.
    /// </summary>
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Tags to apply to the memories extracted from the files uploaded.
    /// </summary>
    public TagCollection Tags { get; set; } = new();

    /// <summary>
    /// Files to process
    /// </summary>
    public List<UploadedFile> Files { get; set; } = new();

    /// <summary>
    /// How to process the files, e.g. how to extract/chunk etc.
    /// </summary>
    public List<string> Steps { get; set; } = new();

    /// <summary>
    /// Normal ctor
    /// </summary>
    public DocumentUploadRequest()
    {
    }

    /// <summary>
    /// Ctor used to translate a <see cref="Document"/> instance to an upload request.
    /// </summary>
    /// <param name="document">Details about the document, e.g. IDs, names, content</param>
    /// <param name="index">Index where to store the memories extracted from the document</param>
    /// <param name="steps">How to process the files, e.g. how to extract/chunk etc.</param>
    public DocumentUploadRequest(Document document, string? index = null, IEnumerable<string>? steps = null)
    {
        this.Index = IndexExtensions.CleanName(index);
        this.Steps = steps?.ToList() ?? new List<string>();

        this.DocumentId = document.Id;
        this.Tags = document.Tags;

        foreach ((string name, Stream content) stream in document.Files.GetStreams())
        {
            var formFile = new UploadedFile(stream.name, stream.content);
            this.Files.Add(formFile);
        }
    }
}
