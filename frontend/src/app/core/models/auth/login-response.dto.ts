export interface LoginResponseDto {
  token: string;
  refreshToken: string;
  username: string;
  expiresInMinutes: number;
}
