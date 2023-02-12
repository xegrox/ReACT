<<<<<<< HEAD
/**
 * Utilites for working with multiple pointer events
 */
export declare function addPointer(pointers: PointerEvent[], event: PointerEvent): void;
export declare function removePointer(pointers: PointerEvent[], event: PointerEvent): void;
/**
 * Calculates a center point between
 * the given pointer events, for panning
 * with multiple pointers.
 */
export declare function getMiddle(pointers: PointerEvent[]): Pick<PointerEvent, "clientX" | "clientY">;
/**
 * Calculates the distance between two points
 * for pinch zooming.
 * Limits to the first 2
 */
export declare function getDistance(pointers: PointerEvent[]): number;
=======
/**
 * Utilites for working with multiple pointer events
 */
export declare function addPointer(pointers: PointerEvent[], event: PointerEvent): void;
export declare function removePointer(pointers: PointerEvent[], event: PointerEvent): void;
/**
 * Calculates a center point between
 * the given pointer events, for panning
 * with multiple pointers.
 */
export declare function getMiddle(pointers: PointerEvent[]): Pick<PointerEvent, "clientX" | "clientY">;
/**
 * Calculates the distance between two points
 * for pinch zooming.
 * Limits to the first 2
 */
export declare function getDistance(pointers: PointerEvent[]): number;
>>>>>>> 3651afdb1032a6a24bd31df54419a655560db9f5
