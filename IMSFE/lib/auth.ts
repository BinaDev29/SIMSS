// src/lib/auth.ts

export interface User {
  username: string
  role: string
}

export interface LoginResponse {
  success: boolean
  message: string
  user?: User
}

export class AuthService {
  private static readonly TOKEN_KEY = "inventory_token"
  private static readonly USER_KEY = "inventory_user"

  static async login(username: string, password: string): Promise<LoginResponse> {
    try {
      // Corrected API endpoint to match your backend's LoginController
      const response = await fetch("http://localhost:5140/api/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      })

      const data = await response.json()

      if (data.success) {
        // Extract the token from the backend's message string
        const tokenMatch = data.message.match(/Token: (.+)/)
        const token = tokenMatch ? tokenMatch[1] : null

        if (token) {
          // In a real application, you'd make a separate API call to get user details from the token.
          // For now, we'll extract from the token's payload to get the role.
          const payload = JSON.parse(atob(token.split('.')[1]));
          const user: User = { username: payload.nameid, role: payload.role };
          
          this.setToken(token);
          this.setUser(user);
          return { success: true, message: data.message, user };
        } else {
          return { success: false, message: "Login successful but token was not received." };
        }
      }

      return { success: false, message: data.message };
    } catch (error) {
      console.error("Login error:", error);
      return { success: false, message: "Network error occurred. Please check your connection." };
    }
  }
  
  // Added the missing 'register' method
  static async register(credentials: { username: string; password: string }) {
    const response = await fetch("http://localhost:5140/api/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(credentials),
    });

    const result = await response.json();
    if (!response.ok) {
        throw new Error(result.message || "Registration failed.");
    }
    return result;
  }

  static logout(): void {
    if (typeof window !== "undefined") {
      localStorage.removeItem(this.TOKEN_KEY);
      localStorage.removeItem(this.USER_KEY);
    }
  }

  static getToken(): string | null {
    if (typeof window === "undefined") return null;
    return localStorage.getItem(this.TOKEN_KEY);
  }

  static getUser(): User | null {
    if (typeof window === "undefined") return null;
    const userStr = localStorage.getItem(this.USER_KEY);
    return userStr ? JSON.parse(userStr) : null;
  }

  static isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  static hasRole(role: string): boolean {
    const user = this.getUser();
    return user?.role === role;
  }

  private static setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  private static setUser(user: User): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }

  static getAuthHeaders(): HeadersInit {
    const token = this.getToken();
    return token ? { Authorization: `Bearer ${token}` } : {};
  }
}