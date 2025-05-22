using IntegrationsJsonPlaceHolder.Domain.Entities;

namespace IntegrationsJsonPlaceHolder.Application.Interfaces
{
    public interface IJsonPlaceHolderService
    {
        // ========== POSTS ==========
        Task<IEnumerable<Post>?> GetPostsAsync();
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post?> CreatePostAsync(Post post);
        Task<Post?> UpdatePostAsync(int id, Post post);
        Task DeletePostAsync(int id);

        // ========== COMMENTS ==========
        Task<IEnumerable<Comment>?> GetCommentsAsync();
        Task<Comment?> GetCommentsByIdAsync(int id);
        Task<IEnumerable<Comment>?> GetCommentsByPostIdAsync(int postId);
        Task<Comment?> CreateCommentAsync(Comment comment);
        Task<Comment?> UpdateCommentAsync(int id, Comment comment);
        Task DeleteCommentAsync(int id);
    }
}
