# Etap 1: Budowanie aplikacji
FROM node:18 AS build

# Ustawienie katalogu roboczego
WORKDIR /app

# Kopiowanie plików package.json i package-lock.json
COPY package*.json ./

# Instalacja zależności
RUN npm install

# Kopiowanie pozostałych plików projektu
COPY . .

# Ustawienie zmiennej środowiskowej
ENV DOCKER=true

# Budowanie aplikacji
RUN npm run build

# Etap 2: Serwowanie aplikacji przy użyciu Nginx
FROM nginx:alpine

# Kopiowanie zbudowanej aplikacji do katalogu Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Eksponowanie portu 80
EXPOSE 80

# Uruchamianie Nginx
CMD ["nginx", "-g", "daemon off;"]
