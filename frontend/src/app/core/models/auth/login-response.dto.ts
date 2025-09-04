export interface LoginResponseDto {
  token: string;
  username: string;
  expiresInMinutes: number;
}
