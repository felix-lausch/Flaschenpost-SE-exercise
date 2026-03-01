namespace API;

public class NoProductsException(string message) : Exception(message);

public class ProductDataException(string message, Exception? inner = null) : Exception(message, inner);