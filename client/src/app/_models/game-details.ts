import { Screenshot } from "./screenshot";

export interface GameDetails {
    id: number;
    name: string;
    coverUrl: string;
    year: number | null;
    genres: string[];
    summary: string;
    screenshots: Screenshot[];
}
