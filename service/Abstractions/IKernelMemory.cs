﻿// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.KernelMemory;

public interface IKernelMemory
{
    /// <summary>
    /// Import a document into memory. The document can contain one or more files, can have tags and other details.
    /// </summary>
    /// <param name="document">Details of the files to import</param>
    /// <param name="index">Optional index name</param>
    /// <param name="steps">Ingestion pipeline steps, optional override to the system default</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Document ID</returns>
    public Task<string> ImportDocumentAsync(
        Document document,
        string? index = null,
        IEnumerable<string>? steps = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Import a file from disk into memory, with details such as tags and user ID.
    /// </summary>
    /// <param name="filePath">Path and name of the file to import</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="tags">Optional tags to apply to the memories generated by the document</param>
    /// <param name="index">Optional index name</param>
    /// <param name="steps">Ingestion pipeline steps, optional override to the system default</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Document ID</returns>
    public Task<string> ImportDocumentAsync(
        string filePath,
        string? documentId = null,
        TagCollection? tags = null,
        string? index = null,
        IEnumerable<string>? steps = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Import a document into memory. The document can contain one or more files, can have tags and other details.
    /// </summary>
    /// <param name="uploadRequest">Upload request containing the document files and details</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Document ID</returns>
    public Task<string> ImportDocumentAsync(
        DocumentUploadRequest uploadRequest,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Import any stream from memory, e.g. text or binary data, with details such as tags and user ID.
    /// </summary>
    /// <param name="content">Content stream to import</param>
    /// <param name="fileName">File name to assign to the stream, used to detect the file type</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="tags">Optional tags to apply to the memories generated by the document</param>
    /// <param name="index">Optional index name</param>
    /// <param name="steps">Ingestion pipeline steps, optional override to the system default</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Document ID</returns>
    public Task<string> ImportDocumentAsync(
        Stream content,
        string? fileName = null,
        string? documentId = null,
        TagCollection? tags = null,
        string? index = null,
        IEnumerable<string>? steps = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Import any stream from memory, e.g. text or binary data, with details such as tags and user ID.
    /// </summary>
    /// <param name="text">Text content to import</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="tags">Optional tags to apply to the memories generated by the document</param>
    /// <param name="index">Optional index name</param>
    /// <param name="steps">Ingestion pipeline steps, optional override to the system default</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Document ID</returns>
    public Task<string> ImportTextAsync(
        string text,
        string? documentId = null,
        TagCollection? tags = null,
        string? index = null,
        IEnumerable<string>? steps = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Import memories from a web page
    /// </summary>
    /// <param name="url">Web page URL</param>
    /// <param name="documentId">Document ID</param>
    /// <param name="tags">Optional tags to apply to the memories generated by the document</param>
    /// <param name="index">Optional index name</param>
    /// <param name="steps">Ingestion pipeline steps, optional override to the system default</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Document ID</returns>
    public Task<string> ImportWebPageAsync(
        string url,
        string? documentId = null,
        TagCollection? tags = null,
        string? index = null,
        IEnumerable<string>? steps = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a list of the indexes available in memory.
    /// </summary>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>List of indexes</returns>
    public Task<IEnumerable<IndexDetails>> ListIndexesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an entire index.
    /// </summary>
    /// <param name="index">Optional index name, when empty the default index is deleted</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    public Task DeleteIndexAsync(
        string? index = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a specified document from memory, and update all derived memories.
    /// </summary>
    /// <param name="documentId">Document ID</param>
    /// <param name="index">Optional index name</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    public Task DeleteDocumentAsync(
        string documentId,
        string? index = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a document ID exists in the given index and is ready for usage.
    /// The logic checks if the uploaded document has been fully processed.
    /// When the document exists in storage but is not processed yet, the method returns False.
    /// </summary>
    /// <param name="documentId">Document ID</param>
    /// <param name="index">Optional index name</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>True if the document has been successfully uploaded and imported</returns>
    public Task<bool> IsDocumentReadyAsync(
        string documentId,
        string? index = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get information about an uploaded document
    /// </summary>
    /// <param name="documentId">Document ID (aka pipeline ID)</param>
    /// <param name="index">Optional index name</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Information about an uploaded document</returns>
    public Task<DataPipelineStatus?> GetDocumentStatusAsync(
        string documentId,
        string? index = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search the given index for a list of relevant documents for the given query.
    /// </summary>
    /// <param name="query">Query to filter memories</param>
    /// <param name="index">Optional index name</param>
    /// <param name="filter">Filter to match</param>
    /// <param name="filters">Filters to match (using inclusive OR logic). If 'filter' is provided too, the value is merged into this list.</param>
    /// <param name="minRelevance">Minimum Cosine Similarity required</param>
    /// <param name="limit">Max number of results to return</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Answer to the query, if possible</returns>
    public Task<SearchResult> SearchAsync(
        string query,
        string? index = null,
        MemoryFilter? filter = null,
        ICollection<MemoryFilter>? filters = null,
        double minRelevance = 0,
        int limit = -1,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search the given index for an answer to the given query.
    /// </summary>
    /// <param name="question">Question to answer</param>
    /// <param name="index">Optional index name</param>
    /// <param name="filter">Filter to match</param>
    /// <param name="filters">Filters to match (using inclusive OR logic). If 'filter' is provided too, the value is merged into this list.</param>
    /// <param name="minRelevance">Minimum Cosine Similarity required</param>
    /// <param name="cancellationToken">Async task cancellation token</param>
    /// <returns>Answer to the query, if possible</returns>
    public Task<MemoryAnswer> AskAsync(
        string question,
        string? index = null,
        MemoryFilter? filter = null,
        ICollection<MemoryFilter>? filters = null,
        double minRelevance = 0,
        CancellationToken cancellationToken = default);
}
