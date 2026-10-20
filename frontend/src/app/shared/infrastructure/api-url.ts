/**
 * Joins the API base URL with an endpoint path, avoiding duplicated or missing slashes.
 */
export function buildApiUrl(apiBaseUrl: string, endpointPath: string): string {
  const base = apiBaseUrl.replace(/\/+$/, '');
  const path = endpointPath.startsWith('/') ? endpointPath : `/${endpointPath}`;
  return `${base}${path}`;
}