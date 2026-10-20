# ---------- Build ----------
FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# ---------- Serve ----------
FROM nginx:1.27-alpine
COPY nginx.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist/maintenance-tickets-web/browser /usr/share/nginx/html

# Railway injects PORT; 8080 is the fallback
ENV PORT=8080
EXPOSE 8080