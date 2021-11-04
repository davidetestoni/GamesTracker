export interface LibraryGameInfo {
    id: number;
    name: string;
    coverUrl: string;
    year: number | null;

    status: UserGameStatus;
    finishedOn: Date | null;
    userRating: number | null;
}

export enum UserGameStatus {
    ToPlay = 0,
    Playing = 1,
    Dropped = 2,
    Finished = 3
}