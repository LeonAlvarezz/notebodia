export type Note = {
    id: string
    userId: string
    title: string
    content?: string
    createdAt: Date
    updatedAt: Date
    publishedAt: Date
}
export type CreateNewNote = {
    title: string
    content: string
}
export type NoteFilter = {
    keyword?: string
}
